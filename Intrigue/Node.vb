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


Public Class Node

    Public Property Nodes As List(Of Node)
    Public Property Atom As Object

    Public Sub New(val As Object)

        Me.Nodes = Nothing
        Me.Atom = val
    End Sub

    Public Sub New(nodes As List(Of Node))

        Me.Nodes = nodes
        Me.Atom = Nothing
    End Sub

    Public Sub New(ParamArray args As Object())

        Me.Nodes = New List(Of Node)
        For Each a In args
            Me.Nodes.Add(New Node(a))
        Next

        Me.Atom = Nothing
    End Sub

    Dim TO_ESCAPE_REGEX As Regex = New Regex("[\s\n""]")

    Public Overrides Function ToString() As String

        If Me.Atom IsNot Nothing Then

            Dim str = TryCast(Me.Atom, String)

            If str Is Nothing Then Return Me.Atom.ToString
            If Not TO_ESCAPE_REGEX.IsMatch(str) Then Return Me.Atom.ToString

            Return """" & str.Replace("""", "\""") & """"
        End If

        Dim s = "("

        For i As Integer = 0 To Me.Nodes.Count - 1
            s = s + Me.Nodes(i).ToString
            If i < Me.Nodes.Count - 1 Then s = s + " "
        Next

        Return s + ")"
    End Function
End Class
