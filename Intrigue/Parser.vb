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


Public Class Parser

    Public Shared Function Parse(s As String) As Node

        Dim t = ParseList(s)

        Return t.Item1
    End Function

    Protected Shared Function DoParse(s As String) As Tuple(Of Node, String)

        s = s.Trim

        If s.StartsWith("(") Then Return ParseList(s)
        If s.StartsWith("'(") Then Return ParseQuotedList(s)
        If s.StartsWith("""") Then Return ParseString(s)

        Dim t As Tuple(Of Node, String)

        t = TryParseNumber(s)
        If t IsNot Nothing Then Return t

        Return ParseSymbol(s)
    End Function

    Protected Shared Function ParseList(s As String) As Tuple(Of Node, String)

        If s.StartsWith("(") Then s = s.Substring(1)

        Dim nodes = New List(Of Node)

        While True

            s = s.Trim

            If s.Length < 1 Then Exit While
            If s.StartsWith(")") Then Exit While

            Dim t = DoParse(s)

            nodes.Add(t.Item1)
            s = t.Item2
        End While

        Return New Tuple(Of Node, String)(New Node(nodes), s)
    End Function

    Protected Shared Function ParseQuotedList(s As String) As Tuple(Of Node, String)

        Return ParseList("quote " + s)
    End Function

    Protected Shared Function ParseString(s As String) As Tuple(Of Node, String)

        s = s.Substring(1)

        Dim r = ""
        Dim escape = False

        While True

            Dim c = s.Substring(0, 1)
            s = s.Substring(1)

            If escape Then
                r = r + c
                escape = False
            ElseIf c = "\" Then
                escape = True
            ElseIf c = """" Then
                Exit While
            Else
                r = r + c
            End If
        End While

        Return New Tuple(Of Node, String)(New Node(r), s)
    End Function

    Protected Shared SYMBOL_REGEX As Regex = New Regex("^([^\s]+)(.*)$")

    Protected Shared Function ParseSymbol(s As String) As Tuple(Of Node, String)

        Dim m = SYMBOL_REGEX.Match(s)

        Return New Tuple(Of Node, String)(New Node(m.Groups(1).ToString), m.Groups(2).ToString)
    End Function

    Protected Shared INTEGER_REGEX As Regex = New Regex("^(-?\d+)(.*)$")

    Protected Shared Function TryParseNumber(s As String) As Tuple(Of Node, String)

        ' TODO: go beyond integers!

        Dim m = INTEGER_REGEX.Match(s)

        If Not m.Success Then Return Nothing

        Dim i = Integer.Parse(m.Groups(1).ToString)

        Return New Tuple(Of Node, String)(New Node(i), m.Groups(2).ToString)
    End Function

    Public Class Node

        Protected Property Nodes As List(Of Node)
        Protected Property Value As Object

        Public Sub New(val As Object)

            Me.Nodes = Nothing
            Me.Value = val
        End Sub

        Public Sub New(nodes As List(Of Node))

            Me.Nodes = nodes
            Me.Value = Nothing
        End Sub

        Public Sub New(ParamArray args As Object())

            Me.Nodes = New List(Of Node)
            For Each a In args
                Me.Nodes.Add(New Node(a))
            Next

            Me.Value = Nothing
        End Sub

        Dim TO_ESCAPE_REGEX As Regex = New Regex("[\s\n""]")

        Public Overrides Function ToString() As String

            If Me.Value IsNot Nothing Then

                Dim str = TryCast(Me.Value, String)

                If str Is Nothing Then Return Me.Value.ToString
                If Not TO_ESCAPE_REGEX.IsMatch(str) Then Return Me.Value.ToString

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
End Class
