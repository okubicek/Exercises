echo "test, test" > generated.txt
for /L %%i in (1,1,28) do type generated.txt >> generated.txt