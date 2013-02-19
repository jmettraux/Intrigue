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

Imports System.Text
Imports System.Text.RegularExpressions


Namespace Nodes

    Public MustInherit Class Node

        Public Function IsList() As Boolean

            Return Me.GetType.ToString.EndsWith("ListNode")
        End Function

        Public Function IsAtom() As Boolean

            Return (TypeOf Me Is AtomNode)
        End Function

        Public Function IsString() As Boolean

            Return IsAtom() AndAlso Not IsSymbol() AndAlso (TypeOf ToAtomNode.Atom Is String)
        End Function

        Public Function IsSymbol() As Boolean

            Return (TypeOf Me Is SymbolNode)
        End Function

        Public Function IsProcedure() As Boolean

            Return (TypeOf Me Is FunctionNode)
        End Function

        Public Function ToAtomNode() As AtomNode

            Try
                Return DirectCast(Me, AtomNode)
            Catch ex As Exception
                Throw New Ex.ArgException("expected atom got " & Me.GetNodeTypeName)
            End Try
        End Function

        Public Function AsSymbolNode() As SymbolNode

            If IsSymbol() Then Return Me
            Return New SymbolNode(Me.ToAtomNode.Atom.ToString)
        End Function

        Public Function ToSymbolNode() As SymbolNode

            Try
                Return DirectCast(Me, SymbolNode)
            Catch ex As Exception
                Throw New Ex.ArgException("expected symbol got " & Me.GetNodeTypeName)
            End Try
        End Function

        Public Function ToListNode() As ListNode

            Try
                Return DirectCast(Me, ListNode)
            Catch ex As Exception
                Throw New Ex.ArgException("expected list got " & Me.GetNodeTypeName)
            End Try
        End Function

        Public Function ToFunctionNode() As FunctionNode

            If Me.IsProcedure Then Return DirectCast(Me, FunctionNode)

            Throw New Ex.ArgException(
                "expected procedure got " & Me.ToString & " (" & Me.GetType.ToString & ")")
        End Function

        Public Function GetNodeTypeName() As String

            Dim s = Me.GetType.ToString.Split(".").Last

            Return s.Substring(0, s.Length - 4).ToLower
        End Function

        Public Overridable Function GetTypeName() As String

            Return GetNodeTypeName()
        End Function

        Public Function ToNumber() As Object

            If Me.ToAtomNode.Atom.ToString.IndexOf(".") > -1 Then
                Return ToDouble()
            Else
                Return ToInteger()
            End If
        End Function

        Public Function ToDouble() As Double

            Return Convert.ToDouble(Me.ToAtomNode.Atom)
        End Function

        Public Function ToInteger() As Long

            Return Convert.ToInt64(Me.ToAtomNode.Atom)
        End Function

        Public Function IsExact() As Boolean

            If Not IsAtom() Then Return False

            Dim type = ToAtomNode.Atom.GetType.ToString.Split(".").Last.ToLower
            If type = "integer" OrElse type = "int32" OrElse type = "int64" Then Return True

            Return False
        End Function

        Public Function IsInexact() As Boolean

            If Not IsAtom() Then Return False

            Dim type = ToAtomNode.Atom.GetType.ToString.Split(".").Last.ToLower
            If type = "float" OrElse type = "double" Then Return True

            Return False
        End Function

        Public Function IsNumber() As Boolean

            Return IsExact() OrElse IsInexact()
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

        Public Overridable Function Eval(ByRef env As Environment) As Node

            Return Me
        End Function

        Public MustOverride Function ToJson() As String
    End Class

    Public Class AtomNode
        Inherits Node

        Public Property Atom As Object

        Public Sub New(ByRef atom As Object)

            Me.Atom = atom
        End Sub

        Public Overrides Function GetTypeName() As String

            If TypeOf Me.Atom Is Int64 Then Return "number"
            If TypeOf Me.Atom Is Double Then Return "number"
            If TypeOf Me.Atom Is Boolean Then Return "boolean"
            Return "string"
        End Function

        Public Overrides Function ToString() As String

            Dim str = Me.Atom.ToString

            If TypeOf Me.Atom Is System.Boolean Then
                Return str.ToLower
            End If
            If TypeOf Me.Atom Is System.Double Then
                If str.IndexOf(".") > -1 Then Return str
                Return str & ".0"
            End If

            If Not (TypeOf Me.Atom Is System.String) Then
                Return str
            End If

            Return """" & str.Replace("""", "\""") & """"
        End Function

        Public Overrides Function ToJson() As String

            Return ToString()
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

        Public Overrides Function GetTypeName() As String

            Return "symbol"
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

        Public Overrides Function Eval(ByRef env As Environment) As Node

            Dim car = Me.Car
            Dim func = env.Eval(car).ToFunctionNode()

            Return func.Apply(car.ToString, Me.Cdr, env)
        End Function

        Public Function Cons(ByRef head As Node)

            Dim r = New ListNode
            r.Push(head)
            For Each n In Me.Nodes
                r.Push(n)
            Next

            Return r
        End Function

        Public Sub Push(ByRef node As Node)

            Me.Nodes.Add(node)
        End Sub

        Public Sub Merge(ByRef list As ListNode)

            Me.Nodes.AddRange(list.Nodes)
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

            If Me.Nodes.Count < 1 Then Return "()"
            Return "(" & String.Join(" ", Me.Nodes.Select(Function(n) n.ToString)) & ")"
        End Function

        Public Overrides Function ToJson() As String

            If Me.Nodes.Count < 1 Then Return "[]"
            Return "[ " & String.Join(", ", Me.Nodes.Select(Function(n) n.ToJson)) & " ]"
        End Function

        Public Overrides Function Inspect() As String

            Return Me.GetType.ToString & ":[" & String.Join(", ", Me.Nodes.Select(Function(n) n.Inspect)) & "]"
        End Function
    End Class

    Public Class ParseListNode
        Inherits ListNode

        Public Sub New(ByRef l As ListNode)

            MyBase.New()
            Me.Nodes = l.Nodes
        End Sub

        Public Overrides Function Eval(ByRef env As Environment) As Node

            Dim r As Node = Nothing
            For Each n In Me.Nodes
                r = env.Eval(n)
            Next

            Return r
        End Function

        Public Overrides Function ToString() As String

            Dim sb = New StringBuilder

            For Each n In Me.Nodes
                sb.Append(n.ToString)
                sb.Append(vbCr)
            Next

            Return sb.ToString.Trim
        End Function
    End Class

    Public MustInherit Class FunctionNode
        Inherits Node

        Public MustOverride Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node
    End Class

    Public MustInherit Class PrimitiveNode
        Inherits FunctionNode

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

        Protected Sub CheckType(type As String, n As Node)

            If type = n.GetTypeName Then Return

            Throw New Ex.ArgException("expected " & type & " got " & n.GetTypeName)
        End Sub

        Protected Sub CheckArgCount(funcName As String, ByRef args As ListNode, minCount As Integer, maxCount As Integer)

            If args.Length >= minCount AndAlso args.Length <= maxCount Then Return

            Throw New Ex.ArgException(
                "'" & funcName & "' expects " & minCount & " to " & maxCount & " arguments, not " & args.Length)
        End Sub

        Public Overrides Function ToJson() As String

            Return """primitive:" & Me.GetType.ToString & """"
        End Function
    End Class

    Public Class EnvironmentNode
        Inherits Node

        Property env As Environment

        Public Sub New(env As Environment)

            MyBase.New()
            Me.env = env
        End Sub

        Public Overrides Function ToJson() As String

            Return """environment"""
        End Function
    End Class
End Namespace
