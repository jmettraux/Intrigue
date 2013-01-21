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


Public Class Interpreter
    Inherits Context

    Public Sub New()

        MyBase.New()
        Populate()
    End Sub

    Public Shared Function DoEval(s As String) As Node

        Return (New Interpreter).Eval(s)
    End Function

    Public Shared Function DoEval(strings As String()) As Node

        Dim s = ""
        For Each ss In strings
            s = s & ss & vbCr
        Next

        Return DoEval(s.Trim)
    End Function

    Protected Sub Populate()

        Bind("quote", New QuoteFunctionNode)

        Bind("car", New CarFunctionNode)
        Bind("cdr", New CdrFunctionNode)
        Bind("cons", New ConsFunctionNode)

        Bind("+", New PlusFunctionNode)

        Bind("define", New DefineFunctionNode)
        Bind("lambda", New LambdaFunctionNode)
    End Sub

    Public Overloads Function Eval(s As String) As Node

        Return Eval(Parser.Parse(s))
    End Function

    Public Overrides Function Eval(ByRef node As Node) As Node

        Dim parseList = TryCast(node, ParseListNode)

        If parseList Is Nothing Then Return MyBase.Eval(node)

        Dim result As Node = Nothing

        For Each n In parseList.Nodes
            result = Eval(n)
        Next

        Return result
    End Function
End Class
