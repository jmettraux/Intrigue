' 
' Copyright (c) 2013-2013, John Mettraux, jmettraux@gmail.com
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' Made in Japan
'

Imports System.Text.RegularExpressions

Namespace Nodes

    Public MustInherit Class Node

        Public Function IsList() As Boolean

            Return Me.GetType.ToString.EndsWith("ListNode")
        End Function

        Public Function IsAtom() As Boolean

            Return Me.GetType.ToString = "Intrigue.Nodes.AtomNode"
        End Function

        Public Function IsSymbol() As Boolean

            Return Me.GetType.ToString = "Intrigue.Nodes.SymbolNode"
        End Function

        Public Function IsFunction() As Boolean

            Return Me.GetType.ToString.EndsWith("FunctionNode")
        End Function

        Public Function ToAtomNode() As AtomNode

            Try
                Return DirectCast(Me, AtomNode)
            Catch ex As Exception
                Throw New Ex.ArgException("expected atom got " & Me.GetTypeName)
            End Try
        End Function

        Public Function ToSymbolNode() As SymbolNode

            Try
                Return DirectCast(Me, SymbolNode)
            Catch ex As Exception
                Throw New Ex.ArgException("expected symbol got " & Me.GetTypeName)
            End Try
        End Function

        Public Function ToListNode() As ListNode

            Try
                Return DirectCast(Me, ListNode)
            Catch ex As Exception
                Throw New Ex.ArgException("expected list got " & Me.GetTypeName)
            End Try
        End Function

        Public Function ToFunctionNode() As FunctionNode

            Try
                Return DirectCast(Me, FunctionNode)
            Catch ex As Exception
                Throw New Ex.ArgException("expected function got " & Me.GetTypeName)
            End Try
        End Function

        Public Function GetTypeName() As String

            Dim s = Me.GetType.ToString

            Return s.Substring(0, s.Length - 4).ToLower
        End Function

        Public Function ToInteger() As Long

            Return Convert.ToInt64(Me.ToAtomNode.Atom)
        End Function

        Public Overridable Function Car() As Node

            Throw New Ex.IntrigueException("'car' only works on lists")
        End Function

        Public Overridable Function Cdr() As ListNode

            Throw New Ex.IntrigueException("'cdr' only works on lists")
        End Function

        Public Overridable Function Inspect() As String

            Return Me.GetType.ToString & ":" & Me.ToString
        End Function
    End Class

    Public Class AtomNode
        Inherits Node

        Public Property Atom As Object

        Public Sub New(ByRef atom As Object)

            Me.Atom = atom
        End Sub

        Public Overrides Function ToString() As String

            If Me.Atom.GetType.ToString = "System.Boolean" Then
                Return Me.Atom.ToString.ToLower
            End If

            Dim str = TryCast(Me.Atom, String)

            If str Is Nothing Then Return Me.Atom.ToString

            Return """" & str.Replace("""", "\""") & """"
        End Function
    End Class

    Public Class SymbolNode
        Inherits AtomNode

        Public Sub New(s As String)

            MyBase.New(s)
        End Sub

        Public Overrides Function ToString() As String

            Return Me.Atom.ToString
        End Function
    End Class

    Public Class ListNode
        Inherits Node

        Public Property Nodes As List(Of Node)

        Public Sub New()

            Me.Nodes = New List(Of Node)
        End Sub

        Public Sub New(ByRef nodes As List(Of Node))

            Me.Nodes = nodes
        End Sub

        Public Sub Push(ByRef node As Node)

            Me.Nodes.Add(node)
        End Sub

        Public Sub Merge(ByRef list As ListNode)

            Me.Nodes.AddRange(list.Nodes)
        End Sub

        ' Used in tests
        '
        Public Sub New(ParamArray args As Object())

            Me.Nodes = New List(Of Node)

            For Each a In args

                Dim n = TryCast(a, Node)

                If n Is Nothing Then
                    Me.Nodes.Add(New AtomNode(a))
                Else
                    Me.Nodes.Add(n)
                End If
            Next
        End Sub

        Public Overrides Function Car() As Node

            Return Me.Nodes(0)
        End Function

        Public Overrides Function Cdr() As ListNode

            Return New ListNode(Me.Nodes.GetRange(1, Me.Nodes.Count - 1))
        End Function

        Public Function Length() As Integer

            Return Me.Nodes.Count
        End Function

        Public Function ToParseListNode() As ParseListNode

            Return New ParseListNode(Me)
        End Function

        Public Overrides Function ToString() As String

            Dim s = "("

            For i As Integer = 0 To Me.Nodes.Count - 1
                s = s + Me.Nodes(i).ToString
                If i < Me.Nodes.Count - 1 Then s = s + " "
            Next

            Return s + ")"
        End Function

        Public Overrides Function Inspect() As String

            Dim s = Me.GetType.ToString & ":["

            For i = 0 To Me.Nodes.Count - 1
                Dim n = Me.Nodes(i)
                s = s & n.Inspect
                If i < Me.Nodes.Count - 1 Then s = s & ", "
            Next

            Return s & "]"
        End Function
    End Class

    Public Class ParseListNode
        Inherits ListNode

        Public Sub New(ByRef l As ListNode)

            MyBase.New()
            Me.Nodes = l.Nodes
        End Sub

        Public Overrides Function ToString() As String

            Dim s = ""

            For Each n In Me.Nodes
                s = s & n.ToString & vbCr
            Next

            Return s.Trim
        End Function
    End Class

    Public MustInherit Class FunctionNode
        Inherits Node

        Public MustOverride Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

        Protected Sub CheckArgCount(funcName As String, ByRef args As ListNode, argCount As Integer)

            If args.Length = argCount Then Return

            Dim m As String
            If argCount = 1 Then
                m = "'" & funcName & "' expects 1 argument, not " & args.Length
            Else
                m = "'" & funcName & "' expects " & argCount & " arguments, not " & args.Length
            End If

            Throw New Ex.ArgException(m)
        End Sub

        Protected Sub CheckArgCount(funcName As String, ByRef args As ListNode, minCount As Integer, maxCount As Integer)

            If args.Length >= minCount AndAlso args.Length <= maxCount Then Return

            Throw New Ex.ArgException(
                "'" & funcName & "' expects " & minCount & " to " & maxCount & " arguments, not " & args.Length)
        End Sub
    End Class

    Public Class EnvironmentNode
        Inherits Node

        Property env As Environment

        Public Sub New(env As Environment)

            MyBase.New()
            Me.env = env
        End Sub
    End Class
End Namespace
