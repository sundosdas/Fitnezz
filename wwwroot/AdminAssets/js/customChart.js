document.addEventListener("DOMContentLoaded", function () {
  const ctx = document.getElementById("subscriptionsChart").getContext("2d");

  // Fetch subscription data
  fetch("/Subscriptions/SubscriptionsChartData")
      .then((response) => {
          if (!response.ok) {
              throw new Error("Failed to fetch chart data");
          }
          return response.json();
      })
      .then((data) => {
          console.log("Fetched data:", data); // Debugging: Log data

          // Check if data is empty
          if (!data || data.length === 0) {
              console.error("No data available for the chart.");
              const noDataMessage = document.createElement("p");
              noDataMessage.textContent = "No subscription data available.";
              document.getElementById("subscriptionsChart").parentNode.appendChild(noDataMessage);
              return;
          }

          const labels = data.map((item) => `${item.month}/${item.year}`); // Ensure lowercase keys
          const counts = data.map((item) => item.count);                  // Ensure lowercase keys

          // Render the chart
          new Chart(ctx, {
              type: "bar",
              data: {
                  labels: labels,
                  datasets: [
                      {
                          label: "Monthly Subscriptions",
                          data: counts,
                          backgroundColor: "rgba(54, 162, 235, 0.6)",
                          borderColor: "rgba(54, 162, 235, 1)",
                          borderWidth: 1,
                      },
                  ],
              },
              options: {
                  responsive: true,
                  plugins: {
                      legend: {
                          display: true,
                          position: "top",
                      },
                  },
                  scales: {
                      x: {
                          title: {
                              display: true,
                              text: "Month",
                          },
                      },
                      y: {
                          title: {
                              display: true,
                              text: "Subscriptions Count",
                          },
                          beginAtZero: true,
                      },
                  },
              },
          });
      })
      .catch((error) => {
          console.error("Error fetching subscription data:", error);
      });
});
