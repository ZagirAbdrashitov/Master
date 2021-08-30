import { createWebHistory, createRouter } from "vue-router";
import Drugs from "@/components/Drugs.vue";
import Data from "@/components/Data.vue";

const routes = [
    {
        path: "/",
        name: "Drugs",
        component: Drugs,
    },
    {
        path: "/Data",
        name: "Data",
        component: Data,
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;