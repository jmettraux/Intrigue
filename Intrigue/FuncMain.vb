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

    Public Class QuoteFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Return args
        End Function
    End Class

    Public Class DefineFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 2)

            Dim car = args.Car

            Dim name As String = Nothing
            Dim value As Node = Nothing

            If car.IsList Then
                name = TryCast(car.Car.ToAtomNode.Atom, String)
                value = New LambdaNode(New ListNode(car.Cdr, args.Cdr.Car))
            ElseIf car.IsSymbol Then
                name = car.ToString
            ElseIf car.IsAtom Then
                name = TryCast(car.ToAtomNode.Atom, String)
            End If

            If name Is Nothing Then
                Throw New Ex.ArgException("'define' expects a Symbol/String as first argument")
            End If

            If value Is Nothing Then
                value = env.Eval(args.Cdr.Car)
            End If

            env.Bind(name, value)

            Return New AtomNode(name)
        End Function
    End Class

    Public Class LambdaNode
        Inherits FunctionNode

        Protected definition As ListNode

        Public Sub New(ByRef definition As ListNode)

            Me.definition = definition
        End Sub

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Dim c = New Environment(env)

            Dim argnames = Me.definition.Car.toListNode.Nodes
            Dim body = Me.definition.Cdr.Car

            For i = 0 To argnames.Count - 1

                Dim argname = argnames(i).ToString
                Dim value = env.Eval(args.Nodes(i))

                c.Bind(argname, value)
            Next

            Return c.Eval(body)
        End Function
    End Class

    Public Class LambdaFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Return New LambdaNode(args)
        End Function
    End Class

    Public Class IfFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            If args.Length < 2 OrElse args.Length > 3 Then
                Throw New Ex.ArgException("'if' expects 2 or 3 arguments, not " & args.Length)
            End If

            Dim pred = env.Eval(args.Car)
            Dim consequence As Node = Nothing

            If pred.ToString = "true" Then
                consequence = args.Cdr.Car
            ElseIf args.Length > 2 Then
                consequence = args.Cdr.Cdr.Car
            Else
                consequence = New AtomNode(False)
            End If

            Return env.Eval(consequence)
        End Function
    End Class

    Public Class CondFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            For Each n In args.Nodes

                If (Not n.IsList) OrElse n.toListNode.Length <> 2 Then
                    Throw New Ex.ArgException("'cond' cannot deal with " & n.ToString)
                End If

                Dim pred = env.Eval(n.Car)
                Dim cons = n.Cdr.Car

                If pred.ToString = "true" Then Return env.Eval(cons)
            Next

            Return New AtomNode(False)
        End Function
    End Class

    Public Class EqualFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 2)

            Dim sa = env.Eval(args.Car).ToString
            Dim sb = env.Eval(args.Cdr.Car).ToString

            Return New AtomNode(sa = sb)
        End Function
    End Class

    Public Class TypeFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1)

            Dim x = env.Eval(args.Car)

            If funcName = "atom?" Then
                Return New AtomNode(x.IsAtom)
            Else ' list?
                Return New AtomNode(x.IsList)
            End If
        End Function
    End Class

    Public Class TheEnvironmentFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 0)

            Return New EnvironmentNode(env)
        End Function
    End Class
End Namespace
