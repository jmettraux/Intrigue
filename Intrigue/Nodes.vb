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


Public MustInherit Class Node

    Public Function IsList() As Boolean

        Return Me.GetType.ToString.EndsWith("ListNode")
    End Function

    Public Function IsAtom() As Boolean

        Return Me.GetType.ToString = "Intrigue.AtomNode"
    End Function

    Public Function IsSymbol() As Boolean

        Return Me.GetType.ToString = "Intrigue.SymbolNode"
    End Function

    Public Function IsFunction() As Boolean

        Return Me.GetType.ToString.EndsWith("FunctionNode")
    End Function

    Public Function ToAtomNode() As AtomNode

        Return DirectCast(Me, AtomNode)
    End Function

    Public Function ToSymbolNode() As SymbolNode

        Return DirectCast(Me, SymbolNode)
    End Function

    Public Function toListNode() As ListNode

        Return DirectCast(Me, ListNode)
    End Function

    Public Overridable Function ToParseList() As ParseListNode

        Return New ParseListNode(Me)
    End Function

    Public Overridable Function Car() As Node

        Throw New IntrigueException("'car' only works on lists")
    End Function

    Public Overridable Function Cdr() As ListNode

        Throw New IntrigueException("'cdr' only works on lists")
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

    ' Used in tests
    '
    Public Sub New(ParamArray args As Object())

        Me.Nodes = New List(Of Node)
        For Each a In args
            Me.Nodes.Add(New AtomNode(a))
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

    Public Sub Push(ByRef node As Node)

        Me.Nodes.Add(node)
    End Sub

    Public Function Compact() As Node

        If Me.Nodes.Count > 1 Then Return Me
        Return Me.Nodes(0)
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

    Public Sub New()

        MyBase.New()
    End Sub

    Public Sub New(ByRef node As Node)

        MyBase.New()
        Me.Nodes.Add(node)
    End Sub

    Public Overrides Function ToParseList() As ParseListNode

        Return Me
    End Function

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

    Public MustOverride Function Apply(ByRef args As ListNode, ByRef context As Context) As Node
End Class
