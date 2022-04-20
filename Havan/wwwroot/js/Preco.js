function AddOrEdit(event) {

    var url = $("#url").val();
    var urlIndex = $("#urlIndex").val();

    event.preventDefault();

    var preco = {
        PrecoHora: $("#PrecoHora").val(),
        PrecoHoraAdicional: $("#PrecoHoraAdicional").val(),
        DataInicial: $("#DataInicial").val(),
        DataFinal: $("#DataFinal").val()
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        ContentType: "application/json; charset=utf-8",
        async: true,
        url: url,
        data: preco,
        success: function (result) {
            if (result.situacao == true)
                location.href = urlIndex;
            else
                alert(result.mensagem);
        },
        error: function (error) {

        }
    });

}


function teste(event) {
    var url = $("#url").val();
    event.preventDefault();

    var preco = {
        PrecoHora: $("#PrecoHora").val(),
        PrecoHoraAdicional: $("#PrecoHoraAdicional").val(),
        DataInicial: $("#DataInicial").val(),
        DataFinal: $("#DataFinal").val()
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        ContentType: "application/json; charset=utf-8",
        async: true,
        url: url,
        data: preco,
        success: function (result) {
            alert("Atualizado com sucesso!");

            window.location.href = '/Preco';
        },
        error: function (error) {

        }
    });
}