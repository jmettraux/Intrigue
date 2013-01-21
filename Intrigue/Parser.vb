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

        Dim parseList = New ParseListNode
        Dim currentList = New ListNode
        Dim r As Result

        While True

            s = s.Trim
            r = DoParse(s)

            If r.HasNode Then currentList.Push(r.Node)
            s = r.Remainder

            If s.Length < 1 OrElse s.StartsWith(vbCr) Then
                parseList.CompactAndPush(currentList)
                currentList = New ListNode
            End If
            If s.Length < 1 Then
                Exit While
            End If
        End While

        Return parseList
    End Function

    Protected Shared Function DoParse(s As String) As Result

        s = s.Trim

        If s.StartsWith("#") Then Return ParseComment(s)
        If s.StartsWith("(") Then Return ParseList(s)
        If s.StartsWith("'(") Then Return ParseList(s)
        If s.StartsWith("""") Then Return ParseString(s)

        Dim r As Result

        r = ParseNumber(s)
        If r.HasNode Then Return r

        r = ParseSymbol(s)
        Return r
    End Function

    Protected Shared COMMENT_REGEX As Regex = New Regex("^([^\r\n]+)")

    Protected Shared Function ParseComment(s As String) As Result

        Dim m = COMMENT_REGEX.Match(s)

        Return New Result(Nothing, s.Substring(m.Groups(1).ToString.Length))
    End Function

    Protected Shared Function ParseList(s As String) As Result

        Dim start = s.Substring(0, 1)

        If s.StartsWith("'(") Then
            start = "("
            s = "quote " & s.Substring(2)
        ElseIf start = "(" Then
            s = s.Substring(1)
        End If

        Dim nodes = New List(Of Node)

        While True

            s = s.Trim

            If start <> "(" AndAlso (s.StartsWith(vbCrLf) OrElse s.StartsWith(vbCr)) Then
                Exit While
            End If

            If s.Length < 1 Then Exit While
            If s.StartsWith(")") Then
                s = s.Substring(1)
                Exit While
            End If

            Dim t = DoParse(s)

            nodes.Add(t.Node)
            s = t.Remainder
        End While

        Return New Result(New ListNode(nodes), s)
    End Function

    Protected Shared Function ParseString(s As String) As Result

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

        Return New Result(New AtomNode(r), s)
    End Function

    Protected Shared SYMBOL_REGEX As Regex = New Regex("^([^\s]+)")

    Protected Shared Function ParseSymbol(s As String) As Result

        Dim m = SYMBOL_REGEX.Match(s)

        If Not m.Success Then Return New Result(Nothing, s)

        Dim sym = m.Groups(1).ToString

        Return New Result(New SymbolNode(sym), s.Substring(sym.Length))
    End Function

    Protected Shared INTEGER_REGEX As Regex = New Regex("^(-?\d+)")

    Protected Shared Function ParseNumber(s As String) As Result

        ' TODO: go beyond integers!

        Dim m = INTEGER_REGEX.Match(s)

        If Not m.Success Then Return New Result(Nothing, s)

        Dim si = m.Groups(1).ToString
        Dim i = Integer.Parse(si)

        Return New Result(New AtomNode(i), s.Substring(si.Length))
    End Function

    Protected Class Result

        Public Property Node As Node
        Public Property Remainder As String

        Public Sub New(ByRef n As Node, ByRef s As String)

            Me.Node = n

            While s.StartsWith(" ")
                s = s.Substring(1)
            End While
            Me.Remainder = s
        End Sub

        Public Function HasNode() As Boolean

            Return (Me.Node IsNot Nothing)
        End Function
    End Class
End Class
