
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

    <TestMethod()> Public Sub Function_the_environment()

        Dim i = New Intrigue.Interpreter
        Dim n = i.Eval("(the-environment)")

        Dim en = TryCast(n, Intrigue.Nodes.EnvironmentNode)
        Assert.AreEqual(i, en.env)
    End Sub

    <TestMethod()> Public Sub Function_eval()

        Assert.AreEqual(
            "10",
            Intrigue.Interpreter.DoEval("eval '(+ 1 2 3 4)").ToString)
    End Sub

    <TestMethod()> Public Sub Function_eval_with_environment()

        Assert.AreEqual(
            "10",
            Intrigue.Interpreter.DoEval("eval '(+ 1 2 3 4) (the-environment)").ToString)

        Assert.AreEqual(
            "10",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define e (the-environment)
                        eval '(+ 1 2 3 4) e
                    ]]>)).ToString)
    End Sub

    <TestMethod()> Public Sub Function_eval_with_other_environment()

        ' TODO: implement let
        ' TODO: implement make-environment shorthand

        Assert.IsTrue(False)
    End Sub

    <TestMethod()> Public Sub Function_let()

        Assert.AreEqual(
            "21",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define x 3
                        let
                          _
                            var0 10
                            var1 11
                          _
                            (+ var0 var1)
                    ]]>)).ToString)
    End Sub
End Class
