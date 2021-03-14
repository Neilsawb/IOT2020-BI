let tabledata = document.getElementById('tabledata')

fetch("http://localhost:7071/api/GetAllFromCosmosDb")
.then(res => res.json())
.then(data => {
    for(let row of data) {
        tabledata.innerHTML += `<tr><td>${row.id}</td><td>${row.Deviceid}</td><td>${row.Date}</td><td>${row.Time}</td><td>${row.temperature}</td><td>${row.humidity}</td><td>${row.Hallvalue}<td>${row.State}</td><td>${row.School}</td><td>${row.Student}</td>`
    }
})
