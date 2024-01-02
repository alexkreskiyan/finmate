format:
	xs format -sc -ic
	dotnet csharpier .

setup:
	xs remote restore -user $(user) -password $(pass)
	dotnet tool restore

update:
	xs update all -sc -ic

clean:
	xs clean -sc -ic

build:
	dotnet build -c Release --nologo -v q

test:
	dotnet test -c Release --no-build --nologo -v q

run:
	cd App && ./bin/Release/net8.0/App > app.log

link:
	xs link ../../annium/backend ../../annium/base -ic -sc
	xs link ../../annium/finance ../../annium/base -ic -sc
	xs link . ../../annium/base -ic -sc
	xs link . ../../annium/backend -ic -sc
	xs link . ../../annium/finance -ic -sc

unlink:
	xs unlink ../../annium/backend ../../annium/base 0.1.0 -ic -sc
	xs unlink ../../annium/finance ../../annium/base 0.1.0 -ic -sc
	xs unlink . ../../annium/base 0.1.0 -ic -sc
	xs unlink . ../../annium/backend 0.1.0 -ic -sc
	xs unlink . ../../annium/finance 0.1.0 -ic -sc



.PHONY: $(MAKECMDGOALS)
