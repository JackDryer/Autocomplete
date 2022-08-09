import re
pattern = re.compile(r"(\w+)\s+\w+\s+(\d+)")
with open("1_2_all_freq.txt","r") as f:
    raw = f.read()
freqpatterns = pattern.findall(raw)
frequencies = {i[0]:int(i[1]) for i in freqpatterns}
with open("en_GB-large.txt","r") as f:
    words = f.read().split()
output = {}

for i in words:
    output [i] = frequencies.get(i,1)
with open("processed.txt", "w") as f:
    for i in output.items():
        f.write(f"{i[0]}:{i[1]}\n")
    
