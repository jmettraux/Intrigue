
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class LibListsTest

    <TestMethod()> Public Sub LibFunc_list()

        Assert.AreEqual(
            "(1 2 3)",
            Interpreter.DoEval("list 1 2 3").ToString)
        Assert.AreEqual(
            "()",
            Interpreter.DoEval("(list)").ToString)
    End Sub

    <TestMethod()> Public Sub LibFunc_foldr()

        Assert.AreEqual(
            "(1 2 3 4)",
            Interpreter.DoEval("foldr cons '() '(1 2 3 4)").ToString)
    End Sub

    <TestMethod()> Public Sub LibFunc_foldl()

        Assert.AreEqual(
            "(4 3 2 1)",
            Interpreter.DoEval("foldl cons '() '(1 2 3 4)").ToString)
    End Sub

    <TestMethod()> Public Sub LibFunc_length()

        Assert.AreEqual(
            "0",
            Interpreter.DoEval("length '()").ToString)
        Assert.AreEqual(
            "1",
            Interpreter.DoEval("length '(1)").ToString)
        Assert.AreEqual(
            "2",
            Interpreter.DoEval("length '(1 ""deux"")").ToString)
    End Sub

    <TestMethod()> Public Sub LibFunc_all()

        Assert.AreEqual(
            "true",
            Interpreter.DoEval("all? (lambda (x) (> x 10)) '()").ToString)

        Assert.AreEqual(
            "false",
            Interpreter.DoEval("all? (lambda (x) (> x 10)) '(4 5 6)").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("all? (lambda (x) (< x 10)) '(4 5 6)").ToString)
    End Sub

    <TestMethod()> Public Sub LibFunc_map()

        Assert.AreEqual(
            "(14 15 16)",
            Interpreter.DoEval("map (lambda (x) (+ x 10)) '(4 5 6)").ToString)
    End Sub

    <TestMethod()> Public Sub LibFunc_map_with_funcname()

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
