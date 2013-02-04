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

            Return args.Car
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

            Dim e = New Environment(env)

            Dim argnames = Me.definition.Car.toListNode.Nodes

            For i = 0 To argnames.Count - 1

                Dim argname = argnames(i).ToString
                Dim value = env.Eval(args.Nodes(i))

                e.Bind(argname, value)
            Next

            Dim result As Node = Nothing
            For Each node In Me.definition.Cdr.Nodes
                result = e.Eval(node)
            Next

            Return result
        End Function

        Public Overrides Function ToString() As String

            Return "lambda: " & Me.definition.ToString
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

            Dim val = env.Eval(args.Car)

            If funcName = "atom?" Then
                Return New AtomNode(val.IsAtom)
            ElseIf funcName = "list?" Then
                Return New AtomNode(val.IsList)
            End If

            Dim type = funcName.Substring(0, funcName.Length - 1)

            Console.WriteLine(val.GetTypeName & " vs " & type)

            Return New AtomNode(val.GetTypeName = type)
        End Function
    End Class

    Public Class TheEnvironmentFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 0)

            Return New EnvironmentNode(env)
        End Function
    End Class

    Public Class MakeEnvironmentFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Dim e = New Environment(env)

            For Each node In args.Nodes
                e.Eval(node)
            Next

            Return New EnvironmentNode(e)
        End Function
    End Class

    Public Class EvalFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            If args.Length < 1 OrElse args.Length > 2 Then
                Throw New Ex.ArgException("'eval' expects 1 or 2 arguments, not " & args.Length)
            End If

            Dim targetEnv = env

            If args.Length > 1 Then
                Dim envNode = env.Eval(args.Cdr.Car)
                targetEnv = DirectCast(envNode, EnvironmentNode).env
            End If

            Dim code = env.Eval(args.Car)

            Return targetEnv.Eval(code)
        End Function
    End Class

    Public Class LetFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            If args.Length < 1 Then
                Throw New Ex.ArgException("'let' expects at least 1 argument")
            End If

            Dim e = New Environment(env)

            Dim name As String
            Dim value As Node
            For Each node In args.Car.ToListNode.Nodes
                name = env.Eval(node.Car, False).ToString
                value = env.Eval(node.Cdr.Car)
                e.Bind(name, value)
            Next

            Dim last As Node = New ListNode ' empty list as nil/null?
            For Each node In args.Cdr.Nodes
                last = e.Eval(node)
            Next

            Return last
        End Function
    End Class

    Public Class AndFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            For Each node In args.Nodes
                Dim v = env.Eval(node)
                If v.IsAtom AndAlso v.ToAtomNode.Atom = False Then Return New AtomNode(False)
            Next

            Return New AtomNode(True)
        End Function
    End Class

    Public Class OrFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Dim r As Boolean = False

            For Each node In args.Nodes
                Dim v = env.Eval(node)
                If Not v.IsAtom Then Return New AtomNode(True)
                If v.IsAtom And v.ToAtomNode.Atom <> False Then Return New AtomNode(True)
            Next

            Return New AtomNode(False)
        End Function
    End Class

    Public Class NotFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1)

            Dim arg = env.Eval(args.Nodes(0))

            Return New AtomNode(arg.IsAtom AndAlso arg.ToAtomNode.Atom = False)
        End Function
    End Class

    Public Class SetFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 2)

            Dim varName = env.Eval(args.Car, False).ToString
            Dim value = env.Eval(args.Cdr.Car)

            env.DoSet(varName, value)

            Return value
        End Function
    End Class
End Namespace
