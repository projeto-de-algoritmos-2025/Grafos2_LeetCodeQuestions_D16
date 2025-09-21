exec: clean build
	./bin/NetworkDelayTime
	./bin/MaximizeSpanningTreeStabilityWithUpgrades

makeBin:
	mkdir -p ./bin

clean: makeBin
	rm -f ./bin/*

build: makeBin
	g++ ./NetworkDelayTime.cpp -o ./bin/NetworkDelayTime
	g++ ./MaximizeSpanningTreeStabilityWithUpgrades.cpp -o ./bin/MaximizeSpanningTreeStabilityWithUpgrades
