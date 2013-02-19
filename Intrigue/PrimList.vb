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

    Public Class CadrPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1)

            Dim current = env.Eval(args.Car)

            For Each c In funcName.Substring(1, funcName.Length - 2).ToCharArray.Reverse
                If c = "a"c Then current = current.Car
                If c = "d"c Then current = current.Cdr
            Next

            Return current
        End Function
    End Class

    Public Class ConsPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 2)

            Dim a = env.Eval(args.Car)
            Dim b = env.Eval(args.Cdr.Car)

            If Not b.IsList Then
                Throw New Ex.ArgException("'cons' expects a list as second argument, not " & b.ToString)
            End If

            Return b.ToListNode.Cons(a)
        End Function
    End Class

    Public Class EmptyPrimitive
        Inherits PrimitiveNode

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
End Namespace
