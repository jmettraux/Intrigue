
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Intrigue


<TestClass()> Public Class LambdaTest

    <TestMethod()> Public Sub Lambda_plus()

        Assert.AreEqual(
            "3",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <string>
                        (define plus (lambda (x y) (+ x y)))
                        plus 1 2
                    </string>)).ToString)
    End Sub

    <TestMethod()> Public Sub Lambda_more()

        Assert.AreEqual(
            "10",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define
                          plus
                          lambda
                            x y
                            + x x
                            + x y
                        plus 3 7
                    ]]>)).ToString)
    End Sub

    <TestMethod()> Public Sub Lambda_list_argument()

        Assert.AreEqual(
            "0",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define kar (lambda l (car l))
                        kar 0 1 2 3
                    ]]>)).ToString)

        Assert.AreEqual(
            "(3 4 5 6)",
            Intrigue.Interpreter.DoEval("(lambda x x) 3 4 5 6").ToString)
    End Sub

    <TestMethod()> Public Sub Lambda_rest_argument()

        Assert.AreEqual(
            "(1 7 8 9)",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        (lambda (x . z) z) 1 1 7 8 9
                    ]]>)).ToString)

        Assert.AreEqual(
            "8",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        (lambda (x . z) (+ x (car (cdr z)))) 1 1 7 8 9
                    ]]>)).ToString)
    End Sub

    <TestMethod()> Public Sub Lambda_dot_rest()

        Assert.AreEqual(
            "(1 7 8 9)",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        (lambda (. z) z) 1 7 8 9
                    ]]>)).ToString)
    End Sub
End Class
