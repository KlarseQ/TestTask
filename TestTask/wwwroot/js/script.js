

async function print() {
    const response = await fetch('api/url/GetAllUrlsInfo')
    const arrUrls = await response.json()
    arrUrls.forEach(item => {
        const div = document.createElement('div')
        div.className = 'table'
        div.innerHTML = `
            <a class="long-link" href="${item.longUrl}">${item.longUrl}</a>
            <a class="short-link" href="${item.shortUrl}">${item.shortUrl}</a>
            <span>${item.linkCount}</span>
            <span>${item.creationDate}</span>
            <button class="btn-delete">Удалить</button>
            <button class="btn-edit">Редактировать</button>
`
        document.querySelector('.table').after(div)
    })
    deleteLink()
    editLink()

}
print()

function deleteLink() {
    const arrBtnsDelete = document.querySelectorAll('.btn-delete')
    arrBtnsDelete.forEach(btn => {
        btn.addEventListener('click', async (e) => {
            const link = e.target.parentElement.querySelector('.short-link').innerText
            const data = new FormData()
            data.append('shortUrl', link)
            const response = await fetch('api/url/RemoveUrlFromDB', {
                method: "POST",
                body: data
            })
            if (response.ok) {
                e.target.parentElement.remove()
            }
        })

    }
    )
}


function editLink() {
    const arrBtnsEdit = document.querySelectorAll('.btn-edit')
    arrBtnsEdit.forEach(btn =>
        btn.addEventListener('click', async (e) => {

            const form = document.querySelector('.modal-content')
            document.querySelector('.modal-wrapper').style.display = 'flex'
            document.querySelector('.oldLongUrl').value = e.target.parentElement.querySelector('.long-link').innerText
            document.querySelector('.btn-edit-submit').addEventListener('click', async (event) => {
                if (form.checkValidity()) {
                    event.preventDefault()
                    const data = new FormData(form)
                    const response = await fetch('api/url/ChangeUrlInfo', {
                        method: "POST",
                        body: data
                    })
                    if (response.ok) {
                        location.reload()
                    } else {

                        alert('Введённый новый URL уже есть в базе')
                    }
                }

            })


        })
    )

}

document.querySelector('.btn-close-modal').addEventListener('click', (e) => {
    e.preventDefault()
    document.querySelector('.modal-wrapper').style.display = 'none'
    document.querySelector('.modal-content').reset()



})

document.querySelector('.changeShortUrl').addEventListener('change', (e) => {
    if (e.target.value === 'true') {
        document.querySelector('.newShortUrlLength').disabled = false
    } if (e.target.value === 'false') {
        document.querySelector('.newShortUrlLength').disabled = true
    }
})