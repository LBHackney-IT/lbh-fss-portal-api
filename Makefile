.PHONY: setup
setup:
	docker-compose build

.PHONY: build
build:
	docker-compose build lbh-fss-portal-api

.PHONY: serve
serve:
	docker-compose build lbh-fss-portal-api && docker-compose up lbh-fss-portal-api

.PHONY: shell
shell:
	docker-compose run lbh-fss-portal-api bash

.PHONY: test
test:
	docker-compose up test-database & docker-compose build lbh-fss-portal-api-test && docker-compose up lbh-fss-portal-api-test

.PHONY: lint
lint:
	-dotnet tool install -g dotnet-format
	dotnet tool update -g dotnet-format
	dotnet format
