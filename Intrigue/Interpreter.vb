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

        Bind("and", New AndPrimitive)
        Bind("or", New OrPrimitive)
        Bind("not", New NotPrimitive)

        Bind("quote", New QuotePrimitive)
        Bind("define", New DefinePrimitive)
        Bind("lambda", New LambdaPrimitive)
        Bind("if", New IfPrimitive)
        Bind("cond", New CondPrimitive)
        Bind("equal?", New EqualPrimitive)
        Bind("set!", New SetPrimitive)

        Bind("atom?", New TypePrimitive)
        Bind("list?", New TypePrimitive)
        Bind("number?", New TypePrimitive)
        Bind("boolean?", New TypePrimitive)
        Bind("string?", New TypePrimitive)
        Bind("exact?", New CheckExactPrimitive)
        Bind("inexact?", New CheckExactPrimitive)

        Bind("car", New CarPrimitive)
        Bind("cdr", New CdrPrimitive)
        Bind("cons", New ConsPrimitive)
        Bind("null?", New EmptyPrimitive)
        Bind("empty?", New EmptyPrimitive)
        Bind("any?", New EmptyPrimitive)

        Bind("+", New PlusPrimitive)
        Bind("-", New MinusPrimitive)
        Bind("*", New StarPrimitive)
        Bind("/", New SlashPrimitive)
        Bind("=", New GreaterPrimitive)
        Bind("<", New GreaterPrimitive)
        Bind("<=", New GreaterPrimitive)
        Bind(">", New GreaterPrimitive)
        Bind(">=", New GreaterPrimitive)
        Bind("max", New MaxMinPrimitive)
        Bind("min", New MaxMinPrimitive)
        Bind("abs", New AbsPrimitive)
        Bind("quotient", New QrmPrimitive)
        Bind("remainder", New QrmPrimitive)
        Bind("modulo", New QrmPrimitive)

        Bind("zero?", New CheckSignPrimitive)
        Bind("positive?", New CheckSignPrimitive)
        Bind("negative?", New CheckSignPrimitive)
        Bind("odd?", New CheckOddnessPrimitive)
        Bind("even?", New CheckOddnessPrimitive)

        Bind("->string", New ToStringPrimitive)
        Bind("symbol->string", New ToStringPrimitive)
        Bind("->exact", New ToInexactPrimitive)
        Bind("->inexact", New ToInexactPrimitive)
        Bind("exact->inexact", New ToInexactPrimitive)
        Bind("inexact->exact", New ToInexactPrimitive)

        Bind("call", New CallPrimitive)

        Bind("eval", New EvalPrimitive)
        Bind("apply", New ApplyPrimitive)
        Bind("let", New LetPrimitive)
        Bind("the-environment", New TheEnvironmentPrimitive)
        Bind("make-environment", New MakeEnvironmentPrimitive)

        EvalLibrary("lists")
        EvalLibrary("math")
        EvalLibrary("json") 'depends on "lists"
    End Sub

    Public Overloads Function Eval(s As String) As Node

        Return Eval(Parsing.Parser.Parse(s))
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
