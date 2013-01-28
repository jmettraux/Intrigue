﻿
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
End Class
