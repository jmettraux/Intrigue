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


Namespace Parsing

    Public Class Token

        Public Property Typ As String
        Public Property Off As Integer
        Public Property Str As String

        Public Sub New(typ As String, off As Integer, s As String)

            Me.Typ = typ
            Me.Off = off
            Me.Str = s
        End Sub

        Public Function Length() As Integer

            Return Me.Str.Length
        End Function

        Public Overrides Function ToString() As String

            Return "(" & Me.Typ & " " & Me.Off & " """ & Me.Str & """)"
        End Function
    End Class

    Public Class Tokenizer

        Protected Shared T_COMMENT As Regex = New Regex("^[ \t]*#[^\r\n]*")
        Protected Shared T_NEWLINE As Regex = New Regex("^[\r\n]+")
        Protected Shared T_SPACE As Regex = New Regex("^[ \t]+")
        Protected Shared T_QUOTE As Regex = New Regex("^'\(")
        Protected Shared T_PAREN As Regex = New Regex("^[\(\)]")
        Protected Shared T_NUMBER As Regex = New Regex("^-?\d[^"" \t\r\n\(\)]*")
        Protected Shared T_SYMBOL As Regex = New Regex("^[^"" \t\r\n\(\)]+")

        Protected Shared Function TokenizeString(s As String, off As Integer) As Token

            Dim ss = s.Substring(off)

            If Not ss.StartsWith("""") Then Return Nothing

            ss = ss.Substring(1)

            Dim r = """"
            Dim escape = False

            While True

                Dim c = ss.Substring(0, 1)
                ss = ss.Substring(1)

                If escape Then
                    r = r + c
                    escape = False
                ElseIf c = "\" Then
                    escape = True
                ElseIf c = """" Then
                    r = r + c
                    Exit While
                Else
                    r = r + c
                End If
            End While

            Return New Token("string", off, r)
        End Function

        Protected Shared Function Tokenize(s As String, off As Integer) As Token

            Dim ss = s.Substring(off)

            Dim m = T_COMMENT.Match(ss)
            If m.Success Then Return New Token("comment", off, m.ToString)

            m = T_NEWLINE.Match(ss)
            If m.Success Then Return New Token("newline", off, m.ToString)

            m = T_SPACE.Match(ss)
            If m.Success Then Return New Token("space", off, m.ToString)

            m = T_QUOTE.Match(ss)
            If m.Success Then Return New Token("quote", off, m.ToString)

            m = T_PAREN.Match(ss)
            If m.Success Then Return New Token(m.ToString, off, m.ToString)

            m = T_NUMBER.Match(ss)
            If m.Success Then Return New Token("number", off, m.ToString)

            Dim t = TokenizeString(s, off)
            If t IsNot Nothing Then Return t

            m = T_SYMBOL.Match(ss)
            If m.Success Then Return New Token("symbol", off, m.ToString)

            Throw New Ex.IntrigueException("cannot parse >" & ss & "<")
        End Function

        Public Shared Function Tokenize(s As String) As List(Of Token)

            Dim l = New List(Of Token)
            Dim off = 0
            Dim t As Token

            While True
                If s.Substring(off).Length < 1 Then Exit While
                t = Tokenize(s, off)
                If t.Typ <> "comment" Then l.Add(t)
                off += t.Length
            End While

            Return l
        End Function
    End Class
End Namespace
