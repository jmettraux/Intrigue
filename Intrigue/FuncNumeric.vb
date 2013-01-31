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

    Public Class PlusFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Dim acc = 0

            For Each n In args.Nodes
                acc += env.Eval(n).ToInteger
            Next

            Return New AtomNode(acc)
        End Function
    End Class

    Public Class MinusFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1, 2)

            Dim acc = env.Eval(args.Car).ToInteger

            For Each n As Node In args.Cdr.Nodes
                acc -= env.Eval(n).ToInteger
            Next

            Return New AtomNode(acc)
        End Function
    End Class

    Public Class StarFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Dim acc = 1

            For Each n In args.Nodes
                acc *= env.Eval(n).ToInteger
            Next

            Return New AtomNode(acc)
        End Function
    End Class


    Public Class GreaterFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 2)

            Dim na = env.Eval(args.Nodes(0)).ToAtomNode
            Dim nb = env.Eval(args.Nodes(1)).ToAtomNode
            Dim ia = Convert.ToInt64(na.Atom)
            Dim ib = Convert.ToInt64(nb.Atom)

            If funcName.EndsWith("=") AndAlso ia = ib Then Return New AtomNode(True)
            If funcName = "=" Then Return New AtomNode(False)

            If funcName.StartsWith("<") Then
                Return New AtomNode(ia < ib)
            Else
                Return New AtomNode(ia > ib)
            End If
        End Function
    End Class
End Namespace
