﻿' 
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

Imports Intrigue.Nodes


Public Class Interpreter
    Inherits Environment

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

        Bind("true", New AtomNode(True))
        Bind("false", New AtomNode(False))
        Bind("#t", Lookup("true"))
        Bind("#f", Lookup("false"))

        Bind("and", New AndFunctionNode)
        Bind("or", New OrFunctionNode)
        Bind("not", New NotFunctionNode)

        Bind("quote", New QuoteFunctionNode)
        Bind("define", New DefineFunctionNode)
        Bind("lambda", New LambdaFunctionNode)
        Bind("if", New IfFunctionNode)
        Bind("cond", New CondFunctionNode)
        Bind("equal?", New EqualFunctionNode)
        Bind("set!", New SetFunctionNode)

        Bind("atom?", New TypeFunctionNode)
        Bind("list?", New TypeFunctionNode)
        Bind("number?", New TypeFunctionNode)
        Bind("boolean?", New TypeFunctionNode)
        Bind("string?", New TypeFunctionNode)
        Bind("exact?", New CheckExactFunctionNode)
        Bind("inexact?", New CheckExactFunctionNode)

        Bind("car", New CarFunctionNode)
        Bind("cdr", New CdrFunctionNode)
        Bind("cons", New ConsFunctionNode)
        Bind("null?", New EmptyFunctionNode)
        Bind("empty?", New EmptyFunctionNode)
        Bind("any?", New EmptyFunctionNode)
        Bind("list", New ListFunctionNode)

        Bind("+", New PlusFunctionNode)
        Bind("-", New MinusFunctionNode)
        Bind("*", New StarFunctionNode)
        Bind("/", New SlashFunctionNode)
        Bind("=", New GreaterFunctionNode)
        Bind("<", New GreaterFunctionNode)
        Bind("<=", New GreaterFunctionNode)
        Bind(">", New GreaterFunctionNode)
        Bind(">=", New GreaterFunctionNode)
        Bind("max", New MaxMinFunctionNode)
        Bind("min", New MaxMinFunctionNode)
        Bind("abs", New AbsFunctionNode)
        Bind("quotient", New QrmFunctionNode)
        Bind("remainder", New QrmFunctionNode)
        Bind("modulo", New QrmFunctionNode)

        Bind("zero?", New CheckSignFunctionNode)
        Bind("positive?", New CheckSignFunctionNode)
        Bind("negative?", New CheckSignFunctionNode)
        Bind("odd?", New CheckOddnessFunctionNode)
        Bind("even?", New CheckOddnessFunctionNode)

        Bind("->string", New ToStringFunctionNode)
        Bind("symbol->string", New ToStringFunctionNode)
        Bind("->exact", New ToInexactFunctionNode)
        Bind("->inexact", New ToInexactFunctionNode)
        Bind("exact->inexact", New ToInexactFunctionNode)
        Bind("inexact->exact", New ToInexactFunctionNode)

        Bind("call", New CallFunctionNode)

        Bind("eval", New EvalFunctionNode)
        Bind("apply", New ApplyFunctionNode)
        Bind("let", New LetFunctionNode)
        Bind("the-environment", New TheEnvironmentFunctionNode)
        Bind("make-environment", New MakeEnvironmentFunctionNode)

        EvalLibrary("lists")
        EvalLibrary("math")
    End Sub

    Public Overloads Function Eval(s As String) As Node

        Return Eval(Parsing.Parser.Parse(s))
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

    Public Sub BindAtom(name As String, ByRef value As Object)

        Me.Bind(name, New AtomNode(value))
    End Sub

    Public Function LookupAtom(name As String) As Object

        Dim node As Node = Me.Lookup(name)

        If node Is Nothing Then Return Nothing
        Return node.ToAtomNode.Atom
    End Function

    Protected Sub EvalLibrary(libName As String)

        Dim code As String

        Using io = System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream("Intrigue." & libName & ".txt")
            Using sr = New System.IO.StreamReader(io)
                code = sr.ReadToEnd
            End Using
        End Using

        Me.Eval(code)
    End Sub
End Class
