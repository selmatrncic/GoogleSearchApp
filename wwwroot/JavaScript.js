let lastResults = [];

document.getElementById("searchButton").addEventListener("click", async function () {

    const keyword = document.getElementById("keyword").value.trim();

    const resultsDiv = document.getElementById("results");
    const downloadButton = document.getElementById("downloadButton");

    resultsDiv.innerHTML = "";
    downloadButton.style.display = "none";

    if (keyword === "") {
        resultsDiv.innerText = "Zadejte prosím klíčové slovo.";
        return;
    }

    const response = await fetch("/api/Search?query=" + encodeURIComponent(keyword));

    const data = await response.json();

    lastResults = data;

    data.forEach((result, index) => {

        const resultDiv = document.createElement("div");

        resultDiv.innerHTML =
            "<h3>" + (index + 1) + ". " + result.title + "</h3>" +
            "<p><a href=\"" + result.link + "\" target=\"_blank\">" +
            result.link +
            "</a></p>" +
            "<p>" + result.description + "</p>";

        resultsDiv.appendChild(resultDiv);
    });

    if (lastResults.length > 0) {
        downloadButton.style.display = "inline-block";
    }
});


document.getElementById("downloadButton").addEventListener("click", function () {

    const json = JSON.stringify(lastResults, null, 2);

    const blob = new Blob([json], {
        type: "application/json"
    });

    const url = URL.createObjectURL(blob);

    const link = document.createElement("a");

    link.href = url;
    link.download = "google-results.json";

    link.click();

    URL.revokeObjectURL(url);
});