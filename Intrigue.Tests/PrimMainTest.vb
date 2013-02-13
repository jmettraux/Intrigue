
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class FuncMainTest

    <TestMethod()> Public Sub Primitive_quote()

        Assert.AreEqual(
            "(1 2 3)",
            Interpreter.DoEval("(quote (1 2 3))").ToString)
        Assert.AreEqual(
            "(1 2 3)",
            Interpreter.DoEval("'(1 2 3)").ToString)
        Assert.AreEqual(
            "1",
            Interpreter.DoEval("(quote 1)").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_if()

        Assert.AreEqual(
            "1",
            Interpreter.DoEval("if (= 7 7) 1 0").ToString)
        Assert.AreEqual(
            "0",
            Interpreter.DoEval("if (= 7 6) 1 0").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_if_when_missing_else()

        Assert.AreEqual(
            "1",
            Interpreter.DoEval("if (= 7 7) 1").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("if (= 7 6) 1").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_cond()

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

    <TestMethod()> Public Sub Primitive_equal()

        Assert.AreEqual(
            "true",
            Intrigue.Interpreter.DoEval("equal? '(1 2) '(1 2)").ToString)
        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("equal? '(1 2) '(3 4)").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_list()

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

    <TestMethod()> Public Sub Primitive_atom()

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

    <TestMethod()> Public Sub Primitive_number()

        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("number? '(1 2)").ToString)
        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("number? true").ToString)
        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("number? ""nada""").ToString)
        Assert.AreEqual(
            "true",
            Intrigue.Interpreter.DoEval("number? 1").ToString)
        Assert.AreEqual(
            "true",
            Intrigue.Interpreter.DoEval("number? -1.45").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_boolean()

        Assert.AreEqual(
            "true",
            Intrigue.Interpreter.DoEval("boolean? true").ToString)
        Assert.AreEqual(
            "true",
            Intrigue.Interpreter.DoEval("boolean? false").ToString)
        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("boolean? 1").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_string()

        Assert.AreEqual(
            "true",
            Intrigue.Interpreter.DoEval("string? ""toto""").ToString)
        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("string? '(1 2)").ToString)
        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("string? 1").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_exact()

        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("exact? ""toto""").ToString)
        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("exact? 1.0").ToString)
        Assert.AreEqual(
            "true",
            Intrigue.Interpreter.DoEval("exact? 1").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_inexact()

        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("inexact? ""toto""").ToString)
        Assert.AreEqual(
            "true",
            Intrigue.Interpreter.DoEval("inexact? 1.0").ToString)
        Assert.AreEqual(
            "false",
            Intrigue.Interpreter.DoEval("inexact? 1").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_the_environment()

        Dim i = New Intrigue.Interpreter
        Dim n = i.Eval("(the-environment)")

        Dim en = TryCast(n, Intrigue.Nodes.EnvironmentNode)
        Assert.AreEqual(i, en.env)
    End Sub

    <TestMethod()> Public Sub Primitive_make_environment()

        Assert.AreEqual(
            "(1 2)",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define a 1
                        define e
                          make-environment
                            define a 2
                        (list a (eval 'a e))
                    ]]>)).ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_eval()

        Assert.AreEqual(
            "10",
            Intrigue.Interpreter.DoEval("eval '(+ 1 2 3 4)").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_eval_with_environment()

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

    <TestMethod()> Public Sub Primitive_apply()

        Assert.AreEqual(
            "6",
            Intrigue.Interpreter.DoEval("apply + '(1 2 3)").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_let()

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

    <TestMethod()> Public Sub Primitive_and()

        Assert.AreEqual("false", Intrigue.Interpreter.DoEval("(and true false)").ToString)
        Assert.AreEqual("true", Intrigue.Interpreter.DoEval("(and true true)").ToString)
        Assert.AreEqual("true", Intrigue.Interpreter.DoEval("(and)").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_or()

        Assert.AreEqual("true", Intrigue.Interpreter.DoEval("(or true false)").ToString)
        Assert.AreEqual("true", Intrigue.Interpreter.DoEval("(or true true)").ToString)
        Assert.AreEqual("false", Intrigue.Interpreter.DoEval("(or false false)").ToString)
        Assert.AreEqual("false", Intrigue.Interpreter.DoEval("(or)").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_not()

        Assert.AreEqual("false", Intrigue.Interpreter.DoEval("(not true)").ToString)
        Assert.AreEqual("true", Intrigue.Interpreter.DoEval("(not false)").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_set_0()

        Assert.AreEqual(
            "1",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        set! a 1
                        a
                    ]]>)).ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_set_1()

        Assert.AreEqual(
            "2",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define a 1
                        define (func0) (set! a 2)
                        define (func1) (define a 3)
                        (func0)
                        (func1)
                        a
                    ]]>)).ToString)
    End Sub
End Class
