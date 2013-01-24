
# intrigue

A mini Scheme for the .NET platform.

Not intended to be complete, awesome or to conquer the world. Just a tool.


## installation

It's available via [NuGET](http://nuget.org/packages/Intrigue/).


## usage

```vb
Imports Intrigue

Intrigue.Interpreter.DoEval(
    Intrigue.Util.NewString(
        <![CDATA[
            (define
              (map f l)
              (if (empty? l)
                l
                (cons (f (car l)) (map f (cdr l)))))
            (map (lambda (x) (+ x 2)) '(10 20 30))
        ]]>)
).ToString

' yields "(12 22 32)"
```


## license

MIT (see LICENSE.txt)

