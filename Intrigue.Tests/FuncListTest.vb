
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class ListFunctionsTest

    <TestMethod()> Public Sub Function_car()

        Assert.AreEqual(
            "1",
            Interpreter.DoEval("car '(1 2 3)").ToString)
        Assert.AreEqual(
            "1",
            Interpreter.DoEval("(car (quote 1 2 3))").ToString)
    End Sub

    <TestMethod()> Public Sub Function_cdr()

        Assert.AreEqual(
            "(2 3)",
            Interpreter.DoEval("cdr '(1 2 3)").ToString)
        Assert.AreEqual(
            "(2 3)",
            Interpreter.DoEval("(cdr (quote 1 2 3))").ToString)
    End Sub

    <TestMethod()> Public Sub Function_cons()

        Assert.AreEqual(
            "(1 2 3)",
            Interpreter.DoEval("cons 1 '(2 3)").ToString)

        Dim e As Exception = Nothing

        Try
            Interpreter.DoEval("cons 3 2")
        Catch e
        End Try

        Assert.AreEqual("Intrigue.Ex.ArgException", e.GetType.ToString)
    End Sub

    <TestMethod()> Public Sub Function_empty()

        Assert.AreEqual(
            "true",
            Interpreter.DoEval("empty? '()").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("empty? '(1)").ToString)
    End Sub

    <TestMethod()> Public Sub Function_any()

        Assert.AreEqual(
            "false",
            Interpreter.DoEval("any? '()").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("any? '(1)").ToString)
    End Sub

    <TestMethod()> Public Sub Function_map()

        Assert.AreEqual(
            "(14 15 16)",
            Interpreter.DoEval("map (lambda (x) (+ x 10)) '(4 5 6)").ToString)
    End Sub

    <TestMethod()> Public Sub Function_map_with_funcname()

        Assert.AreEqual(
            "(22 24 26)",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define (plus10 x) (+ x 10)
                        map plus10 '(12 14 16)
                    ]]>)).ToString)
    End Sub
End Class
