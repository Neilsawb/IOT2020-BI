
fetch("http://localhost:7071/api/GetAllFromCosmosDb")
.then(res => res.json())
.then(data => {
    for(let row of data) {
        document.getElementById('app').innerHTML += `</td><td>${row.deviceid}</td><td>${row.timestamp}</td><td>${row.temperature}</td><td>${row.humidity}</td>`
    }
})
