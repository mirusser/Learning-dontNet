@RD /S /Q "C:\Repos\Learning-dontNet\src\SagaChoreographyPattern\OrderService\deploy"
md "C:\Repos\Learning-dontNet\src\SagaChoreographyPattern\OrderService\"
cd "C:\Repos\Learning-dontNet\src\SagaChoreographyPattern\OrderService\"
dotnet publish  -c Release -o "C:\Repos\Learning-dontNet\src\SagaChoreographyPattern\OrderService\deploy"
docker build -t orderservice .

pause