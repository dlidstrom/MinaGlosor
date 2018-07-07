rem if exist bin\MinaGlosor.Web.dll del /F /S /Q bin\MinaGlosor.Web.dll
rem if exist obj\%1\MinaGlosor.Web.dll del /F /S /Q obj\%1\MinaGlosor.Web.dll
rem if exist cassette-cache FOR /D %%p IN ("cassette-cache\*.*") DO rmdir "%%p" /s /q
