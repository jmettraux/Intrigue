﻿' 
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

    ' TODO: add line to Token[izer]

    Public Class Token

        Public Property Typ As String
        Public Property Off As Integer
        Public Property Tex As String
        Public Property Val As Object

        Public Sub New(typ As String, off As Integer, tex As String, val As Object)

            Me.Typ = typ
            Me.Off = off
            Me.Tex = tex
            Me.Val = val
        End Sub

        Public Function Length() As Integer

            Return Me.Tex.Length
        End Function

        Public Function IsSpace() As Boolean

            Return Me.Typ = "space"
        End Function

        Public Function IsNewline() As Boolean

            Return Me.Typ = "newline"
        End Function

        Public Function IsComment() As Boolean

            Return Me.Typ = "comment"
        End Function

        Public Function IsOpeningParenthesis() As Boolean

            Return Me.Typ.EndsWith("(")
        End Function

        Public Function IsClosingParenthesis() As Boolean

            Return Me.Typ = ")"
        End Function

        Public Overrides Function ToString() As String

            Return "(" & Me.Typ & " " & Me.Off & " """ & Me.Val & """)"
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

            Dim tex = """"
            Dim val = ""
            Dim escape = False

            While True

                Dim c = ss.Substring(0, 1)
                ss = ss.Substring(1)

                tex += c

                If escape Then
                    val += c
                    escape = False
                ElseIf c = "\" Then
                    escape = True
                ElseIf c = """" Then
                    Exit While
                Else
                    val += c
                End If
            End While

            Return New Token("string", off, tex, val)
        End Function

        Protected Shared Function Tokenize(s As String, off As Integer) As Token

            Dim ss = s.Substring(off)

            Dim m = T_COMMENT.Match(ss)
            If m.Success Then Return New Token("comment", off, m.ToString, Nothing)

            m = T_NEWLINE.Match(ss)
            If m.Success Then Return New Token("newline", off, m.ToString, Nothing)

            m = T_SPACE.Match(ss)
            If m.Success Then Return New Token("space", off, m.ToString, Nothing)

            m = T_QUOTE.Match(ss)
            If m.Success Then Return New Token("'(", off, m.ToString, Nothing)

            m = T_PAREN.Match(ss)
            If m.Success Then Return New Token(m.ToString, off, m.ToString, Nothing)

            m = T_NUMBER.Match(ss)
            If m.Success Then Return New Token("number", off, m.ToString, Convert.ToInt64(m.ToString))

            Dim t = TokenizeString(s, off)
            If t IsNot Nothing Then Return t

            m = T_SYMBOL.Match(ss)
            If m.Success Then Return New Token("symbol", off, m.ToString, m.ToString)

            Throw New Ex.IntrigueException("cannot parse >" & ss & "<")
        End Function

        Public Shared Function Tokenize(s As String) As List(Of Token)

            If s.StartsWith("<![CDATA[") Then
                s = s.Substring(9)
                s = s.Substring(0, s.Length - 3)
            End If

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

        Public Shared Function TokensToString(ts As List(Of Token)) As String

            Dim s = "tokens:" & vbCr

            For i = 0 To ts.Count - 1
                s = s & i & " > " & ts(i).ToString & vbCr
            Next

            Return s
        End Function
    End Class
End Namespace