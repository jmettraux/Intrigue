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

    Public Overridable Function IsList() As Boolean

        Return False
    End Function

    Public Overridable Function IsAtom() As Boolean

        Return False
    End Function

    Public Overridable Function ToParseList() As ParseListNode

        Return New ParseListNode(Me)
    End Function
End Class

Public Class AtomNode
    Inherits Node

    Public Property Atom As Object

    Public Sub New(ByRef atom As Object)

        Me.Atom = atom
    End Sub

    Public Overrides Function IsAtom() As Boolean

        Return True
    End Function

    Protected TO_ESCAPE_REGEX As Regex = New Regex("[\s\n""]")

    Public Overrides Function ToString() As String

        Dim str = TryCast(Me.Atom, String)

        If str Is Nothing Then Return Me.Atom.ToString
        If Not TO_ESCAPE_REGEX.IsMatch(str) Then Return Me.Atom.ToString

        Return """" & str.Replace("""", "\""") & """"
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

    Public Overrides Function IsList() As Boolean

        Return True
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

    Public Overrides Function ToString() As String

        Dim s = ""

        For Each n In Me.Nodes
            s = s & n.ToString & vbCr
        Next

        Return s.Trim
    End Function
End Class
