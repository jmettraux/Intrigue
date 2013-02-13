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

    Public Class QuotePrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Return args.Car
        End Function
    End Class

    Public Class DefinePrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 2)

            Dim car = args.Car

            Dim name As String = Nothing
            Dim value As Node = Nothing

            If car.IsList Then

                name = TryCast(car.Car.ToAtomNode.Atom, String)
                Try
                    ' kind of experimental...
                    name = env.Eval(car.Car).ToAtomNode.Atom.ToString
                Catch ex As Exception
                End Try

                value = New LambdaNode(New ListNode(car.Cdr, args.Cdr.Car), env)

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
        Inherits PrimitiveNode

        Protected definition As ListNode
        Protected environment As Environment

        Public Sub New(ByRef definition As ListNode, ByRef env As Environment)

            Me.definition = definition
            Me.environment = env
        End Sub

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Dim e = New Environment(Me.environment)

            Dim car = Me.definition.Car
            If car.IsList Then

                Dim argnames = car.ToListNode.Nodes

                For i = 0 To argnames.Count - 1

                    Dim argname = argnames(i).ToString

                    If argname = "." Then

                        ' collect remaining args...

                        argname = argnames(i + 1).ToString

                        Dim list = New List(Of Node)
                        For j = i To args.Nodes.Count - 1
                            list.Add(env.Eval(args.Nodes(j)))
                        Next

                        e.Bind(argname, New ListNode(list))

                        Exit For

                    Else

                        ' regular case

                        If i > args.Nodes.Count - 1 Then

                            Throw New Ex.ArgException(
                                "not enough arguments, argument '" &
                                argname & "' of " & Me.ToString &
                                " cannot be given a value")
                        End If

                        e.Bind(argname, env.Eval(args.Nodes(i)))
                    End If
                Next

            ElseIf car.IsSymbol Then

                Dim argname = car.ToString

                e.Bind(argname, args)

            Else

                Throw New Ex.ArgException(
                    "'lambda' expects a list of argument names or a single argument name, not a '" & car.GetTypeName & "'")
            End If

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

    Public Class LambdaPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Return New LambdaNode(args, env)
        End Function
    End Class

    Public Class IfPrimitive
        Inherits PrimitiveNode

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

    Public Class CondPrimitive
        Inherits PrimitiveNode

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

    Public Class EqualPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 2)

            Dim sa = env.Eval(args.Car).ToString
            Dim sb = env.Eval(args.Cdr.Car).ToString

            Return New AtomNode(sa = sb)
        End Function
    End Class

    Public Class TypePrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1)

            Dim val = env.Eval(args.Car)

            If funcName = "atom?" Then
                Return New AtomNode(val.IsAtom)
            ElseIf funcName = "list?" Then
                Return New AtomNode(val.IsList)
            End If

            Dim type = funcName.Substring(0, funcName.Length - 1)

            Return New AtomNode(val.GetTypeName = type)
        End Function
    End Class

    Public Class TheEnvironmentPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 0)

            Return New EnvironmentNode(env)
        End Function
    End Class

    Public Class MakeEnvironmentPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Dim e = New Environment(env)

            For Each node In args.Nodes
                e.Eval(node)
            Next

            Return New EnvironmentNode(e)
        End Function
    End Class

    Public Class EvalPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1, 2)

            Dim targetEnv = env

            If args.Length > 1 Then
                Dim envNode = env.Eval(args.Cdr.Car)
                targetEnv = DirectCast(envNode, EnvironmentNode).env
            End If

            Dim code = env.Eval(args.Car)

            Return targetEnv.Eval(code)
        End Function
    End Class

    Public Class ApplyPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 2)

            Dim l = New ListNode(args.Car)

            Dim aas = env.Eval(args.Cdr.Car)

            If Not aas.IsList Then
                Throw New Ex.ArgException(
                    "'apply' expects a list as second argument, not a '" & aas.GetTypeName & "'")
            End If

            l.Nodes.AddRange(aas.ToListNode.Nodes)

            Return env.Eval(l)
        End Function
    End Class

    Public Class LetPrimitive
        Inherits PrimitiveNode

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

    Public Class AndPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            For Each node In args.Nodes
                Dim v = env.Eval(node)
                If v.IsAtom AndAlso v.ToAtomNode.Atom = False Then Return New AtomNode(False)
            Next

            Return New AtomNode(True)
        End Function
    End Class

    Public Class OrPrimitive
        Inherits PrimitiveNode

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

    Public Class NotPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1)

            Dim arg = env.Eval(args.Nodes(0))

            Return New AtomNode(arg.IsAtom AndAlso arg.ToAtomNode.Atom = False)
        End Function
    End Class

    Public Class SetPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 2)

            Dim varName = env.Eval(args.Car, False).ToString
            Dim value = env.Eval(args.Cdr.Car)

            env.DoSet(varName, value)

            Return value
        End Function
    End Class
End Namespace
