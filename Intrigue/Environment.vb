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

Imports Intrigue.Nodes


Public Class Environment

    Protected dic As Dictionary(Of String, Node)
    Protected par As Environment

    Public Sub New()

        dic = New Dictionary(Of String, Node)
    End Sub

    Public Sub New(ByRef parent As Environment)

        Me.New()
        par = parent
    End Sub

    Public Sub Bind(key As String, ByRef node As Node)

        dic(key) = node
    End Sub

    Public Function Lookup(key As String)

        If dic.ContainsKey(key) Then Return dic(key)
        If par Is Nothing Then Throw New Ex.NotFoundException(key)
        Return par.Lookup(key)
    End Function

    Public Overridable Function Eval(ByRef node As Node) As Node

        Return Eval(node, True)
    End Function

    Public Overridable Function Eval(ByRef node As Node, symbolLookup As Boolean) As Node

        If node.IsSymbol Then
            If symbolLookup Then Return Lookup(node.ToString)
            Return node
        End If

        If node.IsAtom Then Return node

        Dim funcName = node.Car.ToString
        Dim func = TryCast(Lookup(funcName), Nodes.FunctionNode)

        If func Is Nothing Then Throw New Ex.NotApplicableException(funcName)

        Return func.Apply(funcName, node.Cdr, Me)
    End Function
End Class
