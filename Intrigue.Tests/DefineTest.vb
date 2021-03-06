﻿
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class DefineTest

    <TestMethod()> Public Sub Define_bind_var()

        Dim i = New Intrigue.Interpreter

        i.Eval("(define a 10)")

        Assert.AreEqual("10", i.Lookup("a").ToString)
    End Sub

    <TestMethod()> Public Sub Define_bind_var_2()

        Assert.AreEqual(
            "12",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <string>
                        define a 12
                        a
                    </string>)).ToString)

        Assert.AreEqual(
            "12",
            Intrigue.Interpreter.DoEval(New String() {
                "define a 12",
                "a"
            }).ToString)
    End Sub

    <TestMethod()> Public Sub Define_bind_dynamic_var()

        Dim i = New Intrigue.Interpreter

        i.Eval("(define ""a"" 10)")

        Assert.AreEqual("10", i.Lookup("a").ToString)
    End Sub

    <TestMethod()> Public Sub Define_function()

        Assert.AreEqual(
            "14",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <string>
                        define (double x) (+ x x)
                        double 7
                    </string>)).ToString)
    End Sub

    <TestMethod()> Public Sub Define_function_with_computed_name()

        ' kind of experimental...

        Assert.AreEqual(
            "14",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                <string>
                    define name "double"
                    define (name x) (+ x x)
                    double 7
                </string>)).ToString)
    End Sub

    <TestMethod()> Public Sub Function_meta_map()

        Assert.AreEqual(
            "(12 22 32)",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        (define
                          (mymap f l)
                          (if (empty? l)
                            l
                            (cons (f (car l)) (mymap f (cdr l)))))
                        (mymap (lambda (x) (+ x 2)) '(10 20 30))
                    ]]>)).ToString)
    End Sub

    <TestMethod()> Public Sub Function_meta_map_lax()

        Assert.AreEqual(
            "(12 22 32)",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define
                          mymap f l
                          if (empty? l)
                            l     ; then
                            cons  ; else
                              f (car l)
                              mymap f (cdr l)
                        mymap
                          lambda (x) (+ x 2)
                          '(10 20 30)
                    ]]>)).ToString)
    End Sub

    <TestMethod()> Public Sub Define_arg_dot_arg()

        Assert.AreEqual(
            "16",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define (f x . z) (+ x (car (cdr z)))
                        f 7 8 9
                    ]]>)).ToString)
    End Sub

    <TestMethod()> Public Sub Define_dot_arg()

        Assert.AreEqual(
            "17",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define (f . z) (+ (car z) (car (cdr (cdr (cdr z)))))
                        f 7 8 9 10
                    ]]>)).ToString)
    End Sub
End Class
