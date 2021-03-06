﻿
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class NumericFunctionsTest

    <TestMethod()> Public Sub Primitive_comparisons()

        Assert.AreEqual(
            "true",
            Interpreter.DoEval("(< 2 3)").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("(> 2 3)").ToString)

        Assert.AreEqual(
            "false",
            Interpreter.DoEval("> 1.2 1.3").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("<= 1.3 1.3").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("<= 1.0 1").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("< 1.2 1.3").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("< 1.2 2").ToString)

        Assert.AreEqual(
            "true",
            Interpreter.DoEval("(<= 3 3)").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("(>= 3 3)").ToString)

        Assert.AreEqual(
            "true",
            Interpreter.DoEval("(= 3 3)").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("(= 3 2)").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_plus()

        Assert.AreEqual(
            "5",
            Interpreter.DoEval("(+ 2 3)").ToString)
        Assert.AreEqual(
            "5",
            Interpreter.DoEval("+ 2 3").ToString)
        Assert.AreEqual(
            "0",
            Interpreter.DoEval("(+)").ToString)
        Assert.AreEqual(
            "2.5",
            Interpreter.DoEval("+ 2 .5").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_minus()

        Assert.AreEqual(
            "5",
            Interpreter.DoEval("- 8 3").ToString)
        Assert.AreEqual(
            "5.0",
            Interpreter.DoEval("- 8 3.0").ToString)

        Dim ex As Exception = Nothing
        Try
            Interpreter.DoEval("- 1 2 3")
            Assert.IsTrue(False)
        Catch ex
        End Try
        Assert.AreEqual("Intrigue.Ex.ArgException", ex.GetType.ToString)
        Assert.AreEqual("'-' expects 1 to 2 arguments, not 3", ex.Message)
    End Sub

    <TestMethod()> Public Sub Primitive_star()

        Assert.AreEqual(
            "-24",
            Interpreter.DoEval("* 8 3 -1").ToString)
        Assert.AreEqual(
            "1",
            Interpreter.DoEval("(*)").ToString)

        Assert.AreEqual(
            "2.4",
            Interpreter.DoEval("* 2 1.2").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_slash()

        Assert.AreEqual(
            "-2",
            Interpreter.DoEval("/ 8 -4").ToString)
        Assert.AreEqual(
            "0",
            Interpreter.DoEval("/ 2").ToString)
        Assert.AreEqual(
            "0.5",
            Interpreter.DoEval("/ 2.0").ToString)

        Dim ex As Exception = Nothing
        Try
            Interpreter.DoEval("(/)")
            Assert.IsTrue(False)
        Catch ex
        End Try
        Assert.AreEqual("Intrigue.Ex.ArgException", ex.GetType.ToString)
        Assert.AreEqual("'/' expects 1 to 2 arguments, not 0", ex.Message)
    End Sub

    <TestMethod()> Public Sub Primitive_zero()

        Assert.AreEqual(
            "false",
            Interpreter.DoEval("zero? '(1 2)").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("zero? 1").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("zero? 1.2").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("zero? 0").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("zero? 0.0").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_positive_negative()

        Assert.AreEqual(
            "false",
            Interpreter.DoEval("positive? '(1 2)").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("positive? -1").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("positive? -1.2").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("positive? 12").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("positive? 15.6").ToString)

        Assert.AreEqual(
            "true",
            Interpreter.DoEval("negative? -1").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_odd_even()

        Assert.AreEqual(
            "false",
            Interpreter.DoEval("odd? '(1 2)").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("odd? 1.0").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("odd? 2").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("odd? 1").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("even? 1").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("even? 2").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_max_min()

        Assert.AreEqual(
            "12",
            Interpreter.DoEval("max -2 5 7 12 1").ToString)
        Assert.AreEqual(
            "-2",
            Interpreter.DoEval("min -2 5 7 12 1").ToString)

        Assert.AreEqual(
            "12.0",
            Interpreter.DoEval("max -2 5 7 12 1.0").ToString)
        Assert.AreEqual(
            "-2.0",
            Interpreter.DoEval("min -2 5 7 12 1.0").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_abs()

        Assert.AreEqual(
            "2",
            Interpreter.DoEval("abs 2").ToString)
        Assert.AreEqual(
            "2.0",
            Interpreter.DoEval("abs 2.0").ToString)
        Assert.AreEqual(
            "2",
            Interpreter.DoEval("abs -2").ToString)
        Assert.AreEqual(
            "2.0",
            Interpreter.DoEval("abs -2.0").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_qrm()

        Assert.AreEqual(
            "1",
            Interpreter.DoEval("quotient 2 2").ToString)
        Assert.AreEqual(
            "3",
            Interpreter.DoEval("quotient 8 3").ToString)

        Assert.AreEqual(
            "0",
            Interpreter.DoEval("remainder 2 2").ToString)
        Assert.AreEqual(
            "1",
            Interpreter.DoEval("remainder 3 2").ToString)
        Assert.AreEqual(
            "1.0",
            Interpreter.DoEval("remainder 3 2.0").ToString)

        Assert.AreEqual(
            "0",
            Interpreter.DoEval("modulo 2 2").ToString)
        Assert.AreEqual(
            "1",
            Interpreter.DoEval("modulo 3 2").ToString)
    End Sub
End Class
