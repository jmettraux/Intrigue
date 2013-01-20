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


Public Class Context
    Inherits Dictionary(Of String, Node)

    Public Property Parent As Context

    Public Sub New()

        MyBase.New()
    End Sub

    Public Sub New(ByRef parent As Context)

        MyBase.New()
        Me.Parent = parent
    End Sub

    Public Function Eval(ByRef node As Node) As Node

        If node.IsAtom Then Return node

        Dim funcName = node.Car.ToString

        If Not ContainsKey(funcName) Then
            Throw New NotFoundException(funcName)
        End If

        Dim func = TryCast(Me(funcName), FunctionNode)

        If func Is Nothing Then
            Throw New NotApplicableException(funcName)
        End If

        Return func.Apply(node.Cdr, Me)
    End Function
End Class

Public Class Interpreter

    Protected Property RootContext As Context

    Public Shared Function DoEval(s As String) As Node

        Dim i = New Interpreter

        Return i.Eval(s)
    End Function

    Public Sub New()

        Me.RootContext = NewRootContext()
    End Sub

    Public Function Eval(s As String) As Node

        Return Eval(Parser.Parse(s))
    End Function

    Public Function Eval(ByRef node As Node) As Node

        Dim result As Node = Nothing

        For Each n In node.ToParseList.Nodes

            result = Me.RootContext.Eval(n)

            '            End If
        Next

        Return result
    End Function

    Protected Function NewRootContext() As Context

        Dim c = New Context

        c("quote") = New QuoteFunctionNode
        c("car") = New CarFunctionNode

        Return c
    End Function
End Class
