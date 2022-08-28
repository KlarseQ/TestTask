const btnGenerate = document.querySelector('.btn-generate')
const btnSaveLink = document.querySelector('.btn-save-link')


btnSaveLink.addEventListener('click', async () => {
    const data = new FormData()

    const longUrl = document.querySelector('.long-link').value
    const shortUrl = document.querySelector('.short-link').innerText

    data.append('longUrl', longUrl)
    data.append('shortUrl', shortUrl)

    const response = await fetch('api/url/AddInfoToDB', {
        method: "POST",
        body: data
    })
    if (!response.ok) {
        alert('Данный длинный URL уже есть в базе')
    }
})


btnGenerate.addEventListener('click', async (e) => {
    e.preventDefault()
    const form = document.getElementById('form')
    const helpText = document.querySelector('.help-text')

    const data = new FormData(form)
    const response = await fetch('api/url/GenerateShortUrl', {
        method: "POST",
        body: data
    })
    if (response.status === 500) {
        helpText.style.display = 'block'
        helpText.innerText = 'Возникла какая-то ошибка, возможно введённый URL уже есть в базе'
        helpText.style.color = 'red'
    }
    if (response.ok) {

        const link = await response.text()
        const shortLink = document.querySelector('.short-link')
        shortLink.innerText = `${link}`
        shortLink.href = `${link}`
        helpText.style.display = 'block'
        helpText.innerText = 'Нажми ещё раз "Сгенерировать ссылку" чтобы изменить её вид'
        helpText.style.color = 'black'
        btnSaveLink.disabled = false

    }

})