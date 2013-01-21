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


Public Class CarFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(ByRef args As ListNode, ByRef context As Context) As Node

        If args.Length <> 1 Then
            Throw New ArgException("'car' expects 1 argument, not " & args.Length)
        End If

        Return context.Eval(args.Car).Car
    End Function
End Class

Public Class CdrFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(ByRef args As ListNode, ByRef context As Context) As Node

        If args.Length <> 1 Then
            Throw New ArgException("'cdr' expects 1 argument, not " & args.Length)
        End If

        Return context.Eval(args.Car).Cdr
    End Function
End Class

Public Class ConsFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(ByRef args As ListNode, ByRef context As Context) As Node

        If args.Length <> 2 Then
            Throw New ArgException("'cons' expects 2 arguments, not " & args.Length)
        End If

        Dim head = context.Eval(args.Car)
        Dim tail = context.Eval(args.Cdr.Car)

        If tail.IsList Then
            tail.toListNode.Nodes.Insert(0, head)
            Return tail
        Else
            Return New ListNode(head, tail)
        End If
    End Function
End Class
