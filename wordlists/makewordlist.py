import re
#pattern = re.compile(r"(\w+)\s+\w+\s+(\d+)")
pattern = re.compile(r"(\w+)\s(\d+)")
with open("enwiki-2022-08-29.txt","r", encoding='utf-8') as f:
    raw = f.read()
freqpatterns = pattern.findall(raw)
oldfrequencies = {i[0]:int(i[1]) for i in freqpatterns}
frequencies = {}
for i in raw.splitlines():
    a,b = i.split()
    frequencies[a] = int(b)
with open("en_GB-large.txt","r") as f:
    words = f.read().split()
output = {}
# all the wordlists havve now loaded.
print("loaded")
# the wikipedia wordlist is all lowercase, the words that hae 2 diffent caseings need to be found first, the old worlist will be used for weigtign
lowerwords = {}
duplicatewords = {}
for i in words:
    if i.lower() in lowerwords:
          lowerwords[i.lower()] = lowerwords[i.lower()] + (i,)
          duplicatewords[i.lower()] = lowerwords[i.lower()]
    else:
        lowerwords[i.lower()] = (i,)

del(lowerwords)
print("duplicates found")
#for i in words:
#    output [i] = frequencies.get(i,1)
for i in words:
    if i in output:
        continue # already added
    if i.lower() in duplicatewords:
        counts = [oldfrequencies.get(x ,1) for x in duplicatewords[i.lower()]]
        totalcounts = sum(counts)
        frequency = frequencies.get(i.lower() ,1)
        if frequency == 1:
            print(duplicatewords[i.lower()])
            frequency = totalcounts
        for x, count in zip (duplicatewords[i.lower()],counts):
            output[x] = int(frequency*count/totalcounts)
    else:
        output[i] = frequencies.get(i.lower() ,1)
print("processed")
with open("processed2.txt", "w") as f:
    for i in output.items():
        f.write(f"{i[0]}:{i[1]}\n")
print("finished")
