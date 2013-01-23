﻿
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class FuncMainTest

    <TestMethod()> Public Sub Function_quote()

        Assert.AreEqual(
            "(1 2 3)",
            Interpreter.DoEval("(quote 1 2 3)").ToString)
        Assert.AreEqual(
            "(1 2 3)",
            Interpreter.DoEval("'(1 2 3)").ToString)
    End Sub

    <TestMethod()> Public Sub Function_if()

        Assert.AreEqual(
            "1",
            Interpreter.DoEval("if (= 7 7) 1 0").ToString)
        Assert.AreEqual(
            "0",
            Interpreter.DoEval("if (= 7 6) 1 0").ToString)
    End Sub

    <TestMethod()> Public Sub Function_if_when_missing_else()

        Assert.AreEqual(
            "1",
            Interpreter.DoEval("if (= 7 7) 1").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("if (= 7 6) 1").ToString)
    End Sub

    <TestMethod()> Public Sub Function_cond()

        Assert.AreEqual(
            """plus""",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define a 7
                        (cond
                          ((= a 6) "six")
                          ((> a 6) "plus")
                          (true "minus"))
                    ]]>)).ToString)

        Assert.AreEqual(
            """minus""",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define a 5
                        (cond
                          ((= a 6) "six")
                          ((> a 6) "plus")
                          (true "minus"))
                    ]]>)).ToString)
    End Sub

    <TestMethod()> Public Sub Function_equal()

        Assert.AreEqual(
            "true",
            Intrigue.Interpreter.DoEval("equal? '(1 2) '(1 2)").ToString)
        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("equal? '(1 2) '(3 4)").ToString)
    End Sub

    <TestMethod()> Public Sub Function_list()

        Assert.AreEqual(
            "true",
            Intrigue.Interpreter.DoEval("list? '(1 2)").ToString)
        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("list? ""a""").ToString)
        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("list? false").ToString)
    End Sub

    <TestMethod()> Public Sub Function_atom()

        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("atom? '(1 2)").ToString)
        Assert.AreEqual(
            "true",
            Intrigue.Interpreter.DoEval("atom? ""a""").ToString)
        Assert.AreEqual(
            "true",
            Intrigue.Interpreter.DoEval("atom? false").ToString)
    End Sub
End Class
