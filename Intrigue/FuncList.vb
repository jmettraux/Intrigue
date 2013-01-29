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


Namespace Nodes

    Public Class CarFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1)

            Return env.Eval(args.Car).Car
        End Function
    End Class

    Public Class CdrFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1)

            Return env.Eval(args.Car).Cdr
        End Function
    End Class

    Public Class ConsFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 2)

            Dim head = env.Eval(args.Car)
            Dim tail = env.Eval(args.Cdr.Car)

            If Not tail.IsList Then
                Throw New Ex.ArgException("'cons' expects a list as second argument, not " & tail.ToString)
            End If

            tail.toListNode.Nodes.Insert(0, head)

            Return tail
        End Function
    End Class

    Public Class EmptyFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1)

            Dim arg = env.Eval(args.Car)

            If Not arg.IsList Then Return New AtomNode(False)

            Dim r = arg.ToListNode.Length < 1
            If funcName = "any?" Then r = Not r

            Return New AtomNode(r)
        End Function
    End Class

    ' Commented out for now, trying to have a small core of functions plus core libs.
    '
    'Public Class MapFunctionNode
    '    Inherits FunctionNode
    '    Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node
    '        CheckArgCount(funcName, args, 2)
    '        Dim func = env.Eval(args.Car).ToFunctionNode
    '        Dim list = env.Eval(args.Cdr.Car).ToListNode
    '        Dim result = New ListNode
    '        For Each node In list.Nodes
    '            result.Push(func.Apply(args.Car.ToString, New ListNode(node), env))
    '        Next
    '        Return result
    '    End Function
    'End Class

    Public Class ListFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Dim l = New ListNode

            For Each node In args.Nodes
                l.Push(env.Eval(node))
            Next

            Return l
        End Function
    End Class
End Namespace
