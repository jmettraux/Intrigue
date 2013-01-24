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

Imports Intrigue.Nodes


Namespace Parsing

    Public Class Parser

        Public Shared Function Parse(s As String) As Node

            Dim tokens = Tokenizer.Tokenize(s)

            'Console.WriteLine(Tokenizer.TokensToString(tokens))

            Return ParseVertical(tokens).toListNode.ToParseListNode
        End Function

        Protected Shared Function ParseVertical(ByRef tokens As List(Of Token)) As Node

            Dim l = New ListNode

            While True
                If tokens.Count < 1 Then Exit While
                l.Push(ParseHorizontal(tokens))
            End While

            Return l
        End Function

        Protected Shared Function ParseHorizontal(ByRef tokens As List(Of Token)) As Node

            Dim t0 = First(tokens)
            Dim n0 = ParsePlain(tokens)

            Dim t As Token

            Dim l = New ListNode(n0)

            While True

                t = First(tokens)

                If t Is Nothing Then Exit While
                If t.Pos.Column <= t0.Nex.Column Then Exit While

                l.Push(ParsePlain(tokens))
                'If t.Pos.Line > t0.Nex.Line Then
                '    l.Push(ParseVertical(tokens))
                'Else
                '    l.Push(ParsePlain(tokens))
                'End If
            End While

            If l.Nodes.Count = 1 Then Return n0

            Return l
        End Function

        Protected Shared Function ParsePlain(ByRef tokens As List(Of Token)) As Node

            Dim t = tokens(0)

            If t.IsClosingParenthesis Then Throw New Ex.UnbalancedParentheseException(t)
            If t.IsOpeningParenthesis Then Return ParseList(tokens)

            Dim token = Shift(tokens)

            If token.IsSymbol Then Return New SymbolNode(token.Val)
            Return New AtomNode(token.Val)
        End Function

        Protected Shared Function ParseList(ByRef tokens As List(Of Token)) As Node

            Dim t As Token
            Dim n As Node
            Dim l As New ListNode

            Dim start = Shift(tokens)
            If start.Typ = "'(" Then l.Push(New SymbolNode("quote"))

            While True
                t = First(tokens)
                If t Is Nothing Then
                    Exit While
                End If
                If t.IsClosingParenthesis Then
                    Shift(tokens)
                    Exit While
                End If
                l.Push(ParsePlain(tokens))
            End While

            Return l
        End Function

        Protected Shared Function First(ByRef tokens As List(Of Token)) As Token

            If tokens.Count < 1 Then Return Nothing
            Return tokens(0)
        End Function

        Protected Shared Function Shift(ByRef tokens As List(Of Token)) As Token

            If tokens.Count < 1 Then Return Nothing

            Dim token = tokens(0)
            tokens.RemoveAt(0)

            Return token
        End Function
    End Class
End Namespace
