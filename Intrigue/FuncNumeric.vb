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


Public Class PlusFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef context As Context) As Node

        Dim i = 0
        Dim v As AtomNode

        For Each n In args.Nodes

            v = TryCast(context.Eval(n), AtomNode)

            If v Is Nothing Then
                Throw New ArgException("'+' cannot add lists")
            End If

            i = i + Convert.ToInt64(v.Atom)
        Next

        Return New AtomNode(i)
    End Function
End Class
