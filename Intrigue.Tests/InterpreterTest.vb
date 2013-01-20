﻿
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class InterpreterTest

    <TestMethod()> Public Sub Interpreter_eval_integer()

        Assert.AreEqual(
            "12",
            Interpreter.DoEval("12").ToString)
    End Sub

    <TestMethod()> Public Sub Interpreter_eval_string()

        Assert.AreEqual(
            "hello",
            Interpreter.DoEval("hello").ToString)
        Assert.AreEqual(
            """hell o""",
            Interpreter.DoEval("""hell o""").ToString)
    End Sub

    <TestMethod()> Public Sub Interpreter_eval_quoted_list()

        Assert.AreEqual(
            "(1 2 3)",
            Interpreter.DoEval("(quote 1 2 3)").ToString)
        Assert.AreEqual(
            "(1 2 3)",
            Interpreter.DoEval("'(1 2 3)").ToString)
    End Sub

    <TestMethod()> Public Sub Interpreter_eval_car()

        Assert.AreEqual(
            "1",
            Interpreter.DoEval("car '(1 2 3)").ToString)
        Assert.AreEqual(
            "1",
            Interpreter.DoEval("(car (quote 1 2 3))").ToString)
    End Sub

    <TestMethod()> Public Sub Interpreter_eval_cdr()

        Assert.AreEqual(
            "(2 3)",
            Interpreter.DoEval("cdr '(1 2 3)").ToString)
        Assert.AreEqual(
            "(2 3)",
            Interpreter.DoEval("(cdr (quote 1 2 3))").ToString)
    End Sub
End Class
