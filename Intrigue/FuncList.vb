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

    Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef context As Context) As Node

        CheckArgCount(funcName, args, 1)

        Return context.Eval(args.Car).Car
    End Function
End Class

Public Class CdrFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(funcName as String, ByRef args As ListNode, ByRef context As Context) As Node

        CheckArgCount(funcName, args, 1)

        Return context.Eval(args.Car).Cdr
    End Function
End Class

Public Class ConsFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef context As Context) As Node

        CheckArgCount(funcName, args, 2)

        Dim head = context.Eval(args.Car)
        Dim tail = context.Eval(args.Cdr.Car)

        If Not tail.IsList Then
            Throw New ArgException("'cons' expects a list as second argument, not " & tail.ToString)
        End If

        tail.toListNode.Nodes.Insert(0, head)

        Return tail
    End Function
End Class

Public Class EmptyFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef context As Context) As Node

        CheckArgCount(funcName, args, 1)

        Dim l = context.Eval(args.Car).toListNode

        Dim r = l.Length < 1
        If funcName = "any?" then r = Not r

        Return New AtomNode(r)
    End Function
End Class
