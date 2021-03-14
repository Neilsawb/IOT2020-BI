let tabledata = document.getElementById('tabledata')

fetch("http://localhost:7071/api/GetDataFromTableStorage")
.then(res => res.json())
.then(data => {
    for(let row of data) {
        tabledata.innerHTML += `<tr><td>${row.rowKey}</td><td>${row.deviceid}</td><td>${row.date}</td><td>${row.time}</td><td>${row.temperature}</td><td>${row.humidity}</td><td>${row.school}</td><td>${row.student}</td>`
    }
})