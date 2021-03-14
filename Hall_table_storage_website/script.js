let tabledata = document.getElementById('tabledata')

fetch("http://localhost:7071/api/GetDataFromTableStorageHall")
.then(res => res.json())
.then(data => {
    for(let row of data) {
        tabledata.innerHTML += `<tr><td>${row.rowKey}</td><td>${row.deviceid}</td><td>${row.date}</td><td>${row.time}</td><td>${row.hallValue}</td><td>${row.state}</td><td>${row.school}</td><td>${row.student}</td>`
    }
})