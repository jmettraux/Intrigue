
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Intrigue


<TestClass()> Public Class LambdaTest

    <TestMethod()> Public Sub Lambda_plus()

        Assert.AreEqual(
            "3",
            Intrigue.Interpreter.DoEval(
                <string>
                    (define plus (lambda (x y) (+ x y)))
                    plus 1 2
                </string>.Nodes.First.ToString.Trim
            ).ToString)
    End Sub
End Class
