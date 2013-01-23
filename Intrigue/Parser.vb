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

Imports Intrigue.Nodes


Namespace Parsing

    Public Class Parser

        Public Shared Function parse(s As String) As Node

            Dim parseList = New ParseListNode
            Dim currentList = New ListNode
            Dim node As Node

            Dim tokens = Tokenizer.Tokenize(s)

            'Console.WriteLine(Tokenizer.TokensToString(tokens))

            While True

                If tokens.Count < 1 OrElse tokens(0).IsNewline Then
                    Shift(tokens)
                    parseList.CompactAndPush(currentList)
                    currentList = New ListNode
                End If

                If tokens.Count < 1 Then Exit While

                node = DoParse(tokens)
                If node IsNot Nothing Then currentList.Push(node)
            End While

            Return parseList
        End Function

        Protected Shared Function Shift(ByRef tokens As List(Of Token)) As Token

            If tokens.Count < 1 Then Return Nothing

            Dim token = tokens(0)
            tokens.RemoveAt(0)

            Return token
        End Function

        Protected Shared Function DoParse(ByRef tokens As List(Of Token)) As Node

            Dim token = Shift(tokens)

            If token.IsSpace Then Return Nothing
            If token.IsNewline Then Return Nothing
            If token.IsComment Then Return Nothing

            If token.IsClosingParenthesis Then Throw New Ex.UnbalancedParentheseException

            If token.IsOpeningParenthesis Then Return ParseList(token, tokens)

            If token.Typ = "symbol" Then Return New SymbolNode(token.Val)

            Return New AtomNode(token.Val)
        End Function

        Protected Shared Function ParseList(ByRef start As Token, ByRef tokens As List(Of Token)) As Node

            Dim plain As Boolean = (start.Tex.IndexOf("(") > -1)
            If start.Tex.StartsWith("'") Then tokens.Insert(0, New Token("symbol", start.Off, "quote", "quote"))

            Dim nodes = New List(Of Node)
            Dim token As Token
            Dim node As Node

            While True

                token = tokens(0)

                If token Is Nothing Then Exit While

                If Not plain AndAlso token.IsNewline Then
                    Shift(tokens)
                    Exit While
                End If
                If plain AndAlso token.IsClosingParenthesis Then
                    Shift(tokens)
                    Exit While
                End If

                node = DoParse(tokens)
                If node IsNot Nothing Then nodes.Add(node)
            End While

            Return New ListNode(nodes)
        End Function
    End Class
End Namespace
